using FishNet.Managing.Scened;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scenes
{

    public sealed class AddressableSceneProcessor : SceneProcessorBase
    {

        /// <summary>
        /// Cached (scene handle, async handles) loaded by the processor, so we can use them for unloading later.
        /// </summary>
        private readonly Dictionary<int, AsyncOperationHandle<SceneInstance>> _loadedScenesByHandle = new(4);
        /// <summary>
        /// Cached loaded scenes known by this processor.
        /// </summary>
        private readonly List<Scene> _loadedScenes = new(4);
        /// <summary>
        /// Current load/unload operations in queue.
        /// </summary>
        private readonly List<AsyncOperationHandle<SceneInstance>> _loadingAsyncOperations = new(4);
        /// <summary>
        /// The current addressables load operation being handled.
        /// </summary>
        private AsyncOperationHandle<SceneInstance> _currentAsyncOperation;
        /// <summary>
        /// Basic async operation for unloading scenes that the processor doesn't know about, i.e. offline scenes.
        /// </summary>
        private AsyncOperation _currentBasicAsyncOperation;
        /// <summary>
        /// Dictionary wrapper of raw scene references so we can get them by scene name.
        /// </summary>
        private Dictionary<string, AssetReference> _compiledAddressableReferences;

        [Tooltip("List of scene names and refs for which we allow loading.")]
        [SerializeField] private List<SceneReference> _rawSceneReferences = new();

        private void Awake()
        {
            _compiledAddressableReferences = new();
            for (int i = 0; i < _rawSceneReferences.Count; i++)
                _compiledAddressableReferences.Add(_rawSceneReferences[i].name.ToLower(), _rawSceneReferences[i].reference);
        }

        public override void LoadStart(LoadQueueData queueData)
        {
            ResetProcessor();
        }

        public override void LoadEnd(LoadQueueData queueData)
        {
            ResetProcessor();
        }

        /// <summary>
        /// Resets performed loading operations for a given load queue.
        /// </summary>
        private void ResetProcessor()
        {
            _currentAsyncOperation = default;
            _loadingAsyncOperations.Clear();
        }

        /// <summary>
        /// Checks if we have a loadable scene in our compiled scene refs.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static bool IsValidScene(string sceneName)
        {
            var proc = FindAnyObjectByType<AddressableSceneProcessor>();
            if (proc == null) return false;

            return proc._compiledAddressableReferences.ContainsKey(sceneName.ToLower());
        }

        public override void BeginLoadAsync(string sceneName, LoadSceneParameters parameters)
        {
            sceneName = Path.GetFileNameWithoutExtension(sceneName);

            // Try get reference
            if (!_compiledAddressableReferences.TryGetValue(sceneName.ToLower(), out AssetReference sceneReference))
                throw new ArgumentException($"Scene with name: {sceneName} is not registered in AddressableSceneProcessor!", nameof(sceneName));

            // load scene with Addressables
            AsyncOperationHandle<SceneInstance> loadHandle = sceneReference.LoadSceneAsync(parameters.loadSceneMode, false);

            // And register this handle in systems
            _loadingAsyncOperations.Add(loadHandle);
            _currentAsyncOperation = loadHandle;
        }

        public override void BeginUnloadAsync(Scene scene)
        {
            if (_loadedScenesByHandle.TryGetValue(scene.handle, out var loadHandle))
            {
                AsyncOperationHandle<SceneInstance> unloadHandle = Addressables.UnloadSceneAsync(loadHandle, false);
                _currentAsyncOperation = unloadHandle;
                _loadedScenes.Remove(scene);
                _loadedScenesByHandle.Remove(scene.handle);
            }
            else
            {
                // maybe it was loaded independently
                _currentBasicAsyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
            }
        }

        public override bool IsPercentComplete()
        {
            bool completed;
            if (_currentBasicAsyncOperation != null)
            {
                completed = GetPercentComplete() >= 0.9;
                if (completed)
                    _currentBasicAsyncOperation = null;
                return completed;
            }
            else if (_currentAsyncOperation.IsValid())
            {
                completed = _currentAsyncOperation.IsDone;

                // cache the scene here since FN scene manager doesnt
                // support addressable handles in their loading loop
                if (completed)
                {
                    AddLoadedScene(_currentAsyncOperation);
                }

                return completed;
            }

            Debug.LogError("Something went wrong", this);
            return false;
        }

        public override float GetPercentComplete()
        {
            float percent = 0f;

            if (_currentBasicAsyncOperation != null)
            {
                percent = _currentBasicAsyncOperation.progress;
            }
            else if (_currentAsyncOperation.IsValid())
            {
                percent = _currentAsyncOperation.PercentComplete;
            }

            return percent;
        }

        // this doesnt even do anything
        public override List<Scene> GetLoadedScenes() => _loadedScenes;

        /// <summary>
        /// Caches a loaded scene.
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="loadHandle">Handle that loaded the scene</param>
        public void AddLoadedScene(AsyncOperationHandle<SceneInstance> loadHandle)
        {
            Scene scene = _currentAsyncOperation.Result.Scene;
            if (_loadedScenesByHandle.ContainsKey(scene.handle))
            {
                Debug.LogWarning("Already added scene with handle: " + scene.handle);
                return;
            }

            _loadedScenes.Add(scene);
            _loadedScenesByHandle.Add(scene.handle, loadHandle);
        }

        public override void ActivateLoadedScenes()
        {
            foreach (var loadingAsyncOp in _loadingAsyncOperations)
            {
                loadingAsyncOp.Result.ActivateAsync();
            }
        }

        public override IEnumerator AsyncsIsDone()
        {
            bool notDone = true;

            while (notDone)
            {
                notDone = false;

                foreach (AsyncOperationHandle<SceneInstance> ao in _loadingAsyncOperations)
                {
                    if (!ao.IsDone)
                    {
                        notDone = true;
                        break;
                    }
                }

                yield return null;
            }
        }

        [Serializable]
        private class SceneReference
        {
            public string name;
            public AssetReference reference;
        }

    }

}
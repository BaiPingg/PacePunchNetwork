using System.Collections.Generic;
using UnityScene = UnityEngine.SceneManagement.Scene;
using System.Collections;
using System;
using FishNet.Managing;
using FishNet.Managing.Scened;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Task = System.Threading.Tasks.Task;

public class CustomSceneProcess : SceneProcessorBase
{
    #region Private.

    /// <summary>
    /// Currently active loading AsyncOperations.
    /// </summary>
    protected List<AsyncOperationHandle<SceneInstance>> LoadingAsyncOperations =
        new List<AsyncOperationHandle<SceneInstance>>();

    /// <summary>
    /// A collection of scenes used both for loading and unloading.
    /// </summary>
    protected List<SceneInstance> Scenes = new List<SceneInstance>();

    /// <summary>
    /// Current AsyncOperation being processed.
    /// </summary>
    protected AsyncOperationHandle<SceneInstance> CurrentAsyncOperation;

    #endregion

  

    /// <summary>
    /// Called when scene unloading has begun within an unload operation.
    /// </summary>
    /// <param name="queueData"></param>
    public override void UnloadStart(UnloadQueueData queueData)
    {
        base.UnloadStart(queueData);
        Scenes.Clear();
    }

    /// <summary>
    /// Begin loading a scene using an async method.
    /// </summary>
    /// <param name="sceneName">Scene name to load.</param>
    public override void BeginLoadAsync(string sceneName,
        UnityEngine.SceneManagement.LoadSceneParameters parameters)
    {
        AsyncOperationHandle<SceneInstance> ao = Addressables.LoadSceneAsync(sceneName, parameters, false);

        LoadingAsyncOperations.Add(ao);
        CurrentAsyncOperation = ao;
    }

    /// <summary>
    /// Begin unloading a scene using an async method.
    /// </summary>
    /// <param name="sceneName">Scene name to unload.</param>
    public override void BeginUnloadAsync(UnityScene scene)
    {
        SceneInstance ss = new SceneInstance();
        foreach (var sce in Scenes)
        {
            if (sce.Scene.name == scene.name)
            {
                ss = sce;
            }
        }

        if (ss.Scene != null)
        {
            Addressables.UnloadSceneAsync(ss);
        }
    }

    /// <summary>
    /// Returns if a scene load or unload percent is done.
    /// </summary>
    /// <returns></returns>
    public override bool IsPercentComplete()
    {
        return (GetPercentComplete() >= 0.9f);
    }


    /// <summary>
    /// Returns the progress on the current scene load or unload.
    /// </summary>
    /// <returns></returns>
    public override float GetPercentComplete()
    {
        return CurrentAsyncOperation.PercentComplete;
    }

    /// <summary>
    /// Adds a loaded scene.
    /// </summary>
    /// <param name="scene">Scene loaded.</param>
    public override void AddLoadedScene(UnityScene scene)
    {
        base.AddLoadedScene(scene);
        Scenes.Add(CurrentAsyncOperation.Result);
    }

    /// <summary>
    /// Returns scenes which were loaded during a load operation.
    /// </summary>
    public override List<UnityScene> GetLoadedScenes()
    {
        List<UnityScene> re = new List<UnityScene>();
        foreach (var scene in Scenes)
        {
            re.Add(scene.Scene);
        }

        return re;
    }

    /// <summary>
    /// Activates scenes which were loaded.
    /// </summary>
    public override void ActivateLoadedScenes()
    {
        for (int i = 0; i < LoadingAsyncOperations.Count; i++)
        {
            try
            {
                LoadingAsyncOperations[i].Result.ActivateAsync().allowSceneActivation = true;
            }
            catch (Exception e)
            {
                base.SceneManager.NetworkManager.LogError($"An error occured while activating scenes. {e.Message}");
            }
        }
    }

    /// <summary>
    /// Returns if all asynchronized tasks are considered IsDone.
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AsyncsIsDone()
    {
        bool notDone;
        do
        {
            notDone = false;
            foreach (AsyncOperationHandle<SceneInstance> ao in LoadingAsyncOperations)
            {
                if (ao.IsValid() && !ao.IsDone)
                {
                    notDone = true;
                    break;
                }
            }

            yield return null;
        } while (notDone);

        yield break;
    }
}
using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ResourcesSerive : MonoBehaviour, IService
{
    public AsyncOperationHandle<T> LoadAsset<T>(string path)
    {
        return Addressables.LoadAssetAsync<T>(path);
    }
    public AsyncOperationHandle<GameObject> LoadAssetAndInstantiate(string path)
    {
        return Addressables.InstantiateAsync(path);
    }
    public  AsyncOperationHandle<T> LoadAssetAsync<T>(string path)
    {
        return Addressables.LoadAssetAsync<T>(path);
        
    }

    public AsyncOperationHandle<GameObject> InstantiateAsync(object path, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
    {
       return Addressables.InstantiateAsync(path,parent,instantiateInWorldSpace,trackHandle);
      
    }
    public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(string path, LoadSceneMode loadMode = LoadSceneMode.Single,
        bool activateOnLoad = true, int priority = 100)
    {
        return Addressables.LoadSceneAsync(path, new LoadSceneParameters(loadMode), activateOnLoad, priority);
    }
}
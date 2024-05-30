using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Object;
using FishNet.Object;
using GameKit.Dependencies.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class NetAddressable : NetworkBehaviour
{
    /// <summary>
    /// Reference to your NetworkManager.
    /// </summary>
    private NetworkManager _networkManager => InstanceFinder.NetworkManager;

    /// <summary>
    /// Used to load and unload addressables in async.
    /// </summary>
    private AsyncOperationHandle<IList<GameObject>> _asyncHandle;

    /// <summary>
    /// Loads an addressables package by string.
    /// You can load whichever way you prefer, this is merely an example.
    /// </summary>
    public IEnumerator LoadAddressables(string addressablesPackage, Action<string, ushort> callback)
    {
        /* FishNet uses an Id to identify addressable packages
         * over the network. You can set the Id to whatever, however
         * you like. A very simple way however is to use our GetStableHash
         * helper methods to return a unique key for the package name.
         * This does require the package names to be unique. */
        ushort id = addressablesPackage.GetStableHashU16();

        /* GetPrefabObjects will return the prefab
         * collection to use for Id. Passing in true
         * will create the collection if needed. */
        SinglePrefabObjects spawnablePrefabs =
            (SinglePrefabObjects)_networkManager.GetPrefabObjects<SinglePrefabObjects>(id, true);

        /* Get a cache to store networkObject references in from our helper object pool.
         * This is not required, you can make a new list if you like. But if you
         * prefer to prevent allocations FishNet has the really helpful CollectionCaches
         * and ObjectCaches, as well Resettable versions of each. */
        List<NetworkObject> cache = CollectionCaches<NetworkObject>.RetrieveList();

        /* Load addressables normally. If the object is a NetworkObject prefab
         * then add it to our cache! */
        _asyncHandle = Addressables.LoadAssetsAsync<GameObject>(addressablesPackage, addressable =>
        {
            NetworkObject nob = addressable.GetComponent<NetworkObject>();
            if (nob != null)
                cache.Add(nob);
        });
        yield return _asyncHandle;

        /* Add the cached references to spawnablePrefabs. You could skip
         * caching entirely and just add them as they are read in our LoadAssetsAsync loop
         * but this saves more performance by adding them all at once. */
        spawnablePrefabs.AddObjects(cache);

        //Optionally(obviously, do it) store the collection cache for use later. We really don't like garbage!
        CollectionCaches<NetworkObject>.Store(cache);
        callback?.Invoke(addressablesPackage, id);
    }

    /// <summary>
    /// Loads an addressables package by string.
    /// </summary>
    public void UnoadAddressables(string addressablesPackage)
    {
        //Get the Id the same was as we did for loading.
        ushort id = addressablesPackage.GetStableHashU16();

        /* Once again get the prefab collection for the Id and
         * clear it so that there are no references of the objects
         * in memory. */
        SinglePrefabObjects spawnablePrefabs =
            (SinglePrefabObjects)_networkManager.GetPrefabObjects<SinglePrefabObjects>(id, true);
        spawnablePrefabs.Clear();
        //You may now release your addressables!
        Addressables.Release(_asyncHandle);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AcknowledgeLoadFinish(ushort ObjectId)
    {
        Debug.Log($"load addressable package finish");
    }


    [ObserversRpc]
    public void LoadAddressPackage(string package)
    {
        Debug.Log($"start load addressable package {package}");

        StartCoroutine(LoadAddressables(package,
            (packageName, ObjectId) => { AcknowledgeLoadFinish(ObjectId); }));
    }

   
    public static void InstantiatePrefab(string path, NetworkConnection connection)
    {
        ushort id = path.GetStableHashU16();
        SinglePrefabObjects spawnablePrefabs =
            (SinglePrefabObjects)InstanceFinder.NetworkManager.GetPrefabObjects<SinglePrefabObjects>(id, true);
        var preabs = Instantiate(spawnablePrefabs.Prefabs[0]);
        InstanceFinder.ServerManager.Spawn(preabs, connection);
    }
}
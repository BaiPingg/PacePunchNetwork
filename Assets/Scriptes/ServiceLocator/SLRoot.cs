using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Component.Spawning;
using FishNet.Object;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SLRoot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SL.Register<TimeService>(FindObjectOfType<TimeService>());
        SL.Register<SteamService>(FindObjectOfType<SteamService>());
        SL.Register<LobbyService>(FindObjectOfType<LobbyService>());
        SL.Register<ProcedureService>(FindObjectOfType<ProcedureService>());
        SL.Register<UIService>(FindObjectOfType<UIService>());
    }

    private void OnEnable()
    {
        InstanceFinder.NetworkManager.GetComponent<PlayerSpawner>().OnSpawned += OnSpawn;
    }

    private void OnDisable()
    {
        InstanceFinder.NetworkManager.GetComponent<PlayerSpawner>().OnSpawned -= OnSpawn;
    }

    private void OnSpawn(NetworkObject obj)
    {
        SL.Register<NetAddressable>(FindObjectOfType<NetAddressable>());
        SL.Register<CampService>(FindObjectOfType<CampService>());
    }
}
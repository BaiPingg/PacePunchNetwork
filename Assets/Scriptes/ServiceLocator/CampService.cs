using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CampService : NetworkBehaviour, IService
{
    [SerializeField] private readonly SyncVar<int> _currentCamp = new();

    public SyncVar<int> CurrentCamp => _currentCamp;


    [ServerRpc(RequireOwnership = false)]
    public void InitializeTheCamp()
    {
        var camps = FindObjectsOfType<CampService>();
        foreach (var ca in camps)
        {
            ca.CurrentCamp.Value = Random.Range(1, 2);
        }
    }

    private void OnEnable()
    {
        _currentCamp.OnChange += OnCampSideChange;
    }


    private void OnDisable()
    {
        _currentCamp.OnChange -= OnCampSideChange;
    }

    private void OnCampSideChange(int prev, int next, bool asserver)
    {
        Debug.Log("camp is ：" + next);
    }
}
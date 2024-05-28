using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorInit : MonoBehaviour
{
    [SerializeField] private GameStartEvent _gameStartEvent;
    
    private void Awake()
    {
        ServiceLocator.Initialize();
    }

    private void Start()
    {
        ServiceLocator.Current.Register(GetComponentInChildren<FileService>());
        ServiceLocator.Current.Register(GetComponentInChildren<ResourcesSerive>());
        ServiceLocator.Current.Register(GetComponentInChildren<ProcedureService>());
        ServiceLocator.Current.Register(GetComponentInChildren<UIService>());
        ServiceLocator.Current.Register(GetComponentInChildren<NetService>());


        Debug.Log("ServiceLocator init finish");
        _gameStartEvent.Raise(null);
     
    }
}
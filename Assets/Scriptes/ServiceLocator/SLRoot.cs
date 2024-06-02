using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SLRoot : MonoBehaviour
{
  
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SL.Register<SteamService>(FindObjectOfType<SteamService>());
        SL.Register<LobbyService>(FindObjectOfType<LobbyService>());
        SL.Register<ProcedureService>(FindObjectOfType<ProcedureService>());
        SL.Register<UIService>(FindObjectOfType<UIService>());
    }
}
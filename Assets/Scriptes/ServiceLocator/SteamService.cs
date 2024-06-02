using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;

public class SteamService : MonoBehaviour, IService
{
    public void OnSteamInit()
    {
        Debug.Log($"[{GetType().Name}]: SteamWork init finished!");
      
    }
    
}
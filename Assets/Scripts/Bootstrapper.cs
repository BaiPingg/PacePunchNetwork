using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    private void Awake()
    {
        Addressables.LoadSceneAsync("Assets/Scenes/MainMenu.unity", LoadSceneMode.Additive);
    }
}

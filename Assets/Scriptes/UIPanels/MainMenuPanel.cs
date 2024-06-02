using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class MainMenuPanel : UIPanel
{
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private Button _lobbyListsBtn;
    [SerializeField] private Button _optionsBtn;
    [SerializeField] private Button _quitBtn;

    public override void OnOpen(UIService service)
    {
        base.OnOpen(service);
        _createLobbyBtn.onClick.AddListener(CreateLobby);
        _lobbyListsBtn.onClick.AddListener(OpenLobbiesLists);
        _quitBtn.onClick.AddListener(QuitGame);
    }


    public override void OnClose()
    {
        base.OnClose();
        _createLobbyBtn.onClick.RemoveAllListeners();
        _lobbyListsBtn.onClick.RemoveAllListeners();
        _quitBtn.onClick.RemoveAllListeners();
    }

    private  void OpenLobbiesLists()
    {
        Addressables.InstantiateAsync(nameof(LobbyListsPanel)).Completed += handle =>
        {
            SL.Get<UIService>().OpenPanel(handle.Result.GetComponent<UIPanel>());
        };
      
    }

    private void CreateLobby()
    {
       SL.Get<LobbyService>().CreateLobby(4,true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
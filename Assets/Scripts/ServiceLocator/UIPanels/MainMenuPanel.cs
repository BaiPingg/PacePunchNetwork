using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    public override void OnClose()
    {
        base.OnClose();
        _createLobbyBtn.onClick.RemoveAllListeners();
        _lobbyListsBtn.onClick.RemoveAllListeners();
    }
    private async void OpenLobbiesLists()
    {
      await  ServiceLocator.Current.Get<UIService>().OpenPanel("Assets/Prefabs/LobbyListsPanel.prefab");
    }
    private void CreateLobby()
    {
       ServiceLocator.Current.Get<NetService>().StartHost(4);
      
    }
}
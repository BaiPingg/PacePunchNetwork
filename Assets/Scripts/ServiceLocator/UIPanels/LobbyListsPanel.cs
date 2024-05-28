using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListsPanel : UIPanel
{
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _joinBtn;

    [SerializeField] private Button _refreshBtn;
    [SerializeField] private GameObject _lobbyListContent;
    [SerializeField] private LobbyCard _lobbyCardTemplate;

    private Lobby? currentLobby;

    public override async void OnOpen(UIService service)
    {
        base.OnOpen(service);


        _backBtn.onClick.AddListener(BackToMainMenu);
        _joinBtn.onClick.AddListener(JoinLobby);
        _refreshBtn.onClick.AddListener(RefreshList);
        NetworkManager.Singleton.OnClientStarted += OnClientStarted;
        RefreshList();
    }


    public override void OnClose()
    {
        base.OnClose();
        _backBtn.onClick.RemoveAllListeners();
        _joinBtn.onClick.RemoveAllListeners();
        _refreshBtn.onClick.RemoveAllListeners();
        NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
    }

    private async void OnClientStarted()
    {
        ServiceLocator.Current.Get<UIService>().ClosePanel();
        await ServiceLocator.Current.Get<UIService>().OpenPanel("Assets/Prefabs/LobbyPanel.prefab");
    }


    private async void RefreshList()
    {
        for (int i = 0; i < _lobbyListContent.transform.childCount; i++)
        {
            Destroy(_lobbyListContent.transform.GetChild(i).gameObject);
        }

        Lobby[] lobbies = await SteamMatchmaking.LobbyList.WithMaxResults(20).WithKeyValue("white", "white")
            .RequestAsync();
        if (lobbies == null || lobbies.Length == 0)
            return;
        foreach (var lobby in lobbies)
        {
            var lobbyCard = Instantiate(_lobbyCardTemplate.gameObject, _lobbyListContent.transform)
                .GetComponent<LobbyCard>();
            lobbyCard.Init(lobby);
            lobbyCard.GetComponent<Button>().onClick.AddListener(() => { currentLobby = lobby; });
        }
    }

    private void JoinLobby()
    {
        if (currentLobby != null)
        {
            currentLobby.Value.Join();
        }
    }

    private void BackToMainMenu()
    {
        ServiceLocator.Current.Get<UIService>().ClosePanel();
    }
}
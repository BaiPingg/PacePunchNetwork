using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : UIPanel
{
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _StartBtn;
    [SerializeField] private Button _ReadyBtn;
    [SerializeField] private GameObject _playerListContent;
    [SerializeField] private PlayerCard _playerCardTemplate;
    [SerializeField] private ChatMessage _chatMessageTemplate;

    public Dictionary<ulong, PlayerCard> _PlayerCards = new Dictionary<ulong, PlayerCard>();
    private PlayerCard _currentPlayer;
    public List<ChatMessage> _ChatMessages = new List<ChatMessage>();

    public override void OnOpen(UIService service)
    {
        base.OnOpen(service);

        _StartBtn.gameObject.SetActive(SL.Get<LobbyService>().IsOwner());
        _backBtn.onClick.AddListener(OnBackBtnClick);
        _ReadyBtn.onClick.AddListener(SetPlayerReady);
        _StartBtn.onClick.AddListener(RequestStartGame);
    }

    private void RequestStartGame()
    {
        bool allReady = true;
        foreach (var player in _PlayerCards)
        {
            if (player.Value.ready == false)
            {
                allReady = false;
            }
        }

        if (allReady)
        {
        }
    }


    public override void OnClose()
    {
        base.OnClose();
        _backBtn.onClick.RemoveAllListeners();
        _StartBtn.onClick.RemoveAllListeners();
        _ReadyBtn.onClick.RemoveAllListeners();
    }

    private void SetPlayerReady()
    {
        if (_currentPlayer != null)
        {
            // var player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<NetPlayer>();
            // player.RequestSetMemberIsReadyServerRpc(NetworkManager.Singleton.LocalClientId, !_currentPlayer.ready);
        }
    }

    private void OnBackBtnClick()
    {
        SL.Get<LobbyService>().LeaveCurrentLobby();
        SL.Get<UIService>().ClosePanel();
    }

    public void OnLobbyChatEvent(LobbyChatMeassage chatMeassage)
    {
        var chatMessage = Instantiate(_chatMessageTemplate.gameObject).GetComponent<ChatMessage>();
        chatMessage.Init(chatMeassage);
        _ChatMessages.Add(chatMessage);
    }

    public void OnAddPlayerToPlayerLists(PlayerBaseInfo playerInfo)
    {
        if (_PlayerCards.ContainsKey(playerInfo.clientId))
        {
            return;
        }

        var card = Instantiate(_playerCardTemplate.gameObject, _playerListContent.transform)
            .GetComponent<PlayerCard>();
        card.Init(playerInfo);
        _PlayerCards.Add(playerInfo.clientId, card);
        // if (NetworkManager.Singleton.LocalClient.ClientId == playerInfo.clientId)
        // {
        //     _currentPlayer = card;
        // }
    }

    public void OnRemoveFormPlayerLists(PlayerBaseInfo playerInfo)
    {
        if (_PlayerCards.ContainsKey(playerInfo.clientId))
        {
            var target = _PlayerCards[playerInfo.clientId];
            _PlayerCards.Remove(playerInfo.clientId);
            Destroy(target.gameObject);
        }

        // if (NetworkManager.Singleton.LocalClient.ClientId == playerInfo.clientId)
        // {
        //     ServiceLocator.Current.Get<NetService>().DisConnected();
        //     ServiceLocator.Current.Get<UIService>().ClosePanel();
        // }
    }

    public void UpdateClients()
    {
        foreach (var player in _PlayerCards)
        {
            ulong _steamId = player.Value.steamId;
            string _steamName = player.Value.steamName;
            ulong _clientId = player.Key;
            // NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<NetPlayer>()
            //     .UpdateClientsPlayerInfoClientRPC(_steamId, _steamName,
            //         _clientId);
        }
    }

    // public void OnsetPlayerReadyState(PlayerReadySet playerReadySet)
    // {
    //     foreach (var player in _PlayerCards)
    //     {
    //         if (player.Value.cilentId == playerReadySet.clientId)
    //         {
    //             player.Value.SetReady(playerReadySet.ready);
    //         }
    //     }
    // }
}
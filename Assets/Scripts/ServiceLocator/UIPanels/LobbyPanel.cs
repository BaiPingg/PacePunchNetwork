using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
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
    private Lobby? currentLobby;
    public Dictionary<ulong, PlayerCard> _PlayerCards = new Dictionary<ulong, PlayerCard>();
    public List<ChatMessage> _ChatMessages = new List<ChatMessage>();

    public override void OnOpen(UIService service)
    {
        base.OnOpen(service);
        currentLobby = ServiceLocator.Current.Get<NetService>().currentLobby;
        if (currentLobby == null)
        {
            Debug.LogError("errror ,lobby  is null");
            return;
        }

        //only host can start
        _StartBtn.gameObject.SetActive(currentLobby.Value.Owner.IsMe);
        _backBtn.onClick.AddListener(OnBackBtnClick);
    }

    public override void OnClose()
    {
        base.OnClose();
        _backBtn.onClick.RemoveAllListeners();
    }


    private void OnBackBtnClick()
    {
        ServiceLocator.Current.Get<NetService>().DisConnected();
        ServiceLocator.Current.Get<UIService>().ClosePanel();
    }

    public void OnLobbyChatEvent(LobbyChatMeassage chatMeassage)
    {
        var chatMessage = Instantiate(_chatMessageTemplate.gameObject).GetComponent<ChatMessage>();
        chatMessage.Init(chatMeassage);
        _ChatMessages.Add(chatMessage);
    }

    public void OnAddPlayerToPlayerLists(AddPlayer2PlayerList playerInfo)
    {
        var card = Instantiate(_playerCardTemplate.gameObject, _playerListContent.transform)
            .GetComponent<PlayerCard>();
        card.Init(playerInfo);
        _PlayerCards.Add(playerInfo.clientId, card);
    }

    void UpdateClients()
    {
        foreach (var player in _PlayerCards)
        {
            ulong _steamId = player.Value.steamId;
            string _steamName = player.Value.steamName;
            ulong _clientId = player.Key;
            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<NetPlayer>().RequestAddMeToDictionaryServerRPC
                (SteamClient.SteamId,SteamClient.Name,NetworkManager.Singleton.LocalClientId);
        }
    }
}
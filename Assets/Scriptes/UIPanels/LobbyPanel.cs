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
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdate;
    protected Callback<LobbyChatUpdate_t> LobbyChatUpdate;

    public override void OnOpen(UIService service)
    {
        base.OnOpen(service);
        LobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
        _StartBtn.gameObject.SetActive(false);
        _backBtn.onClick.AddListener(OnBackBtnClick);
        _ReadyBtn.onClick.AddListener(SetPlayerReady);
        _StartBtn.onClick.AddListener(RequestStartGame);
        InitMember();
    }

    private void OnLobbyChatUpdate(LobbyChatUpdate_t arg)
    {
        if (arg.m_ulSteamIDLobby == SL.Get<LobbyService>().CurrentLobbyID)
        {
            var state = (EChatMemberStateChange)arg.m_rgfChatMemberStateChange;
            if (state == EChatMemberStateChange.k_EChatMemberStateChangeEntered)
            {
                Debug.Log($"{arg.m_ulSteamIDUserChanged} joined");
                AddMember(arg.m_ulSteamIDLobby, arg.m_ulSteamIDUserChanged);
            }
            else
            {
                RemoveMember(arg.m_ulSteamIDLobby, arg.m_ulSteamIDUserChanged);
                Debug.Log($"{arg.m_ulSteamIDUserChanged} left");
            }
        }
    }

    private void RemoveMember(ulong argMUlSteamIDLobby, ulong argMUlSteamIDUserChanged)
    {
        if (_PlayerCards.ContainsKey(argMUlSteamIDUserChanged))
        {
            var target = _PlayerCards[argMUlSteamIDUserChanged];
            _PlayerCards.Remove(argMUlSteamIDUserChanged);
            Destroy(target);
        }
    }

    private void AddMember(ulong argMUlSteamIDLobby, ulong argMUlSteamIDUserChanged)
    {
        var card = Instantiate(_playerCardTemplate.gameObject, _playerListContent.transform)
            .GetComponent<PlayerCard>();
        var playerInfo = new PlayerBaseInfo();
        playerInfo.steamName = SteamFriends.GetFriendPersonaName(new CSteamID(argMUlSteamIDUserChanged));
        playerInfo.streamId = argMUlSteamIDUserChanged;
        card.Init(playerInfo);
        _PlayerCards.Add(playerInfo.streamId, card);
    }

    private void OnLobbyDataUpdate(LobbyDataUpdate_t dataUpdated)
    {
        if (dataUpdated.m_ulSteamIDLobby == dataUpdated.m_ulSteamIDMember)
        {
            //It was lobby data that was updated
        }
        else
        {
            //It was this member that updated
            var ready = SteamMatchmaking.GetLobbyMemberData(new CSteamID(dataUpdated.m_ulSteamIDLobby),
                new CSteamID(dataUpdated.m_ulSteamIDMember), "Ready");
            if (bool.TryParse(ready, out bool result))
            {
                _PlayerCards[dataUpdated.m_ulSteamIDMember].SetReady(result);
            }

            CheckStartBtn();
        }
    }

    private void CheckStartBtn()
    {
        _StartBtn.gameObject.SetActive(SL.Get<LobbyService>().IsOwner() &&
                                       SL.Get<LobbyService>().IsAllMemberReady());
    }


    private void InitMember()
    {
        var lobby = new CSteamID(SL.Get<LobbyService>().CurrentLobbyID);
        var count = SteamMatchmaking.GetNumLobbyMembers(lobby);
        for (int i = 0; i < count; i++)
        {
            var member = SteamMatchmaking.GetLobbyMemberByIndex(lobby, i);
            AddMember(SL.Get<LobbyService>().CurrentLobbyID, member.m_SteamID);
        }
    }

    private void RequestStartGame()
    {
        var loading = new LoadingGameState();
        SL.Get<ProcedureService>().SwitchState(loading);
    }


    public override void OnClose()
    {
        base.OnClose();
        _backBtn.onClick.RemoveAllListeners();
        _StartBtn.onClick.RemoveAllListeners();
        _ReadyBtn.onClick.RemoveAllListeners();
        LobbyDataUpdate.Unregister();
    }

    private void SetPlayerReady()
    {
        var ready = SteamMatchmaking.GetLobbyMemberData(new CSteamID(SL.Get<LobbyService>().CurrentLobbyID),
            SteamUser.GetSteamID(), "Ready");
        if (bool.TryParse(ready, out bool result))
        {
            SteamMatchmaking.SetLobbyMemberData(new CSteamID(SL.Get<LobbyService>().CurrentLobbyID), "Ready",
                (!result).ToString());
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
using System;
using FishNet.Managing;
using Steamworks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CSteamID = Steamworks.CSteamID;

public class LobbyService : MonoBehaviour, IService
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;
    protected Callback<LobbyChatUpdate_t> LobbyChatUpdate;

    public ulong CurrentLobbyID;


    private void Start()
    {
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        LobbyChatUpdate = new Callback<LobbyChatUpdate_t>(OnLobbyChatUpdate);
    }

    private void OnLobbyChatUpdate(LobbyChatUpdate_t arg)
    {
        if (arg.m_ulSteamIDLobby == CurrentLobbyID)
        {
            var state = (EChatMemberStateChange)arg.m_rgfChatMemberStateChange;
            if (state == EChatMemberStateChange.k_EChatMemberStateChangeEntered)
            {
                Debug.Log($"{arg.m_ulSteamIDUserChanged} joined");
            }
            else
            {
                Debug.Log($"{arg.m_ulSteamIDUserChanged} left");
            }
        }
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
            return;

        CurrentLobbyID = callback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "GameName", "FindAndSeek");

        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "Name",
            SteamFriends.GetPersonaName().ToString() + "'s lobby");


        _fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        _fishySteamworks.StartConnection(true);
        Debug.Log($"Lobby creation was successful {CurrentLobbyID}");
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        _fishySteamworks.SetClientAddress(
            SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress"));
        _fishySteamworks.StartConnection(false);
        Addressables.InstantiateAsync(nameof(LobbyPanel)).Completed += handle =>
        {
            SL.Get<UIService>().OpenPanel(handle.Result.GetComponent<UIPanel>());
        };
    }

    public void FindLobbies()
    {
        SteamMatchmaking.AddRequestLobbyListStringFilter("GameName", "FindAndSeek",
            ELobbyComparison.k_ELobbyComparisonEqual);
        SteamMatchmaking.RequestLobbyList();
    }

    public void JoinLobby(CSteamID steamID)
    {
        if (SteamMatchmaking.RequestLobbyData(steamID))
            SteamMatchmaking.JoinLobby(steamID);
    }

    public void CreateLobby(int maxNumber, bool isPublic)
    {
        SteamMatchmaking.CreateLobby(isPublic ? ELobbyType.k_ELobbyTypePublic : ELobbyType.k_ELobbyTypeFriendsOnly,
            maxNumber);
    }

    public bool IsOwner()
    {
        var owner = SteamMatchmaking.GetLobbyOwner(new CSteamID(CurrentLobbyID));
        return owner == SteamUser.GetSteamID();
    }

    public void LeaveCurrentLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyID));
        CurrentLobbyID = 0;

        _fishySteamworks.StopConnection(false);
        if (_networkManager.IsServerStarted)
            _fishySteamworks.StopConnection(true);
    }
}
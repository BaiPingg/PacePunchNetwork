using System;
using UnityEngine;
using Unity.Netcode;
using Steamworks;
using Steamworks.Data;
using Netcode.Transports.Facepunch;


public class NetService : MonoBehaviour, IService
{
    private FacepunchTransport transport = null;
    public Lobby? currentLobby { get; protected set; } = null;


    private void Start()
    {
        transport = NetworkManager.Singleton.gameObject.GetComponent<FacepunchTransport>();
        SteamMatchmaking.OnLobbyCreated += SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite += SteamMatchmaking_OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated += SteamMatchmaking_OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested += SteamFriends_OnGameLobbyJoinRequested;
       
    }

    private void OnDestroy()
    {
        SteamMatchmaking.OnLobbyCreated -= SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite -= SteamMatchmaking_OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated -= SteamMatchmaking_OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested -= SteamFriends_OnGameLobbyJoinRequested;
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        NetworkManager.Singleton.OnServerStarted -= Singleton_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= Singleton_OnClientDisconnectCallback;
    }

    private void OnApplicationQuit()
    {
        DisConnected();
    }

    //
    private async void SteamFriends_OnGameLobbyJoinRequested(Lobby _lobby, SteamId _steamId)
    {
        RoomEnter joinedLobby = await _lobby.Join();
        if (joinedLobby != RoomEnter.Success)
        {
            Debug.Log("Failed to create lobby");
        }
        else
        {
            currentLobby = _lobby;

            Debug.Log("Joined lobby");
        }
    }

    private void SteamMatchmaking_OnLobbyGameCreated(Lobby _lobby, uint _ip, ushort _port, SteamId _steamId)
    {
        Debug.Log("Lobby was created");
    }

    private void SteamMatchmaking_OnLobbyInvite(Friend _steamId, Lobby _lobby)
    {
        Debug.Log($"Invate from {_steamId.Name}");
    }

    private void SteamMatchmaking_OnLobbyMemberLeave(Lobby _lobby, Friend _steamId)
    {
        Debug.Log($"Member leave: {_steamId.Name}");
    }

    private void SteamMatchmaking_OnLobbyMemberJoined(Lobby _lobby, Friend _steamId)
    {
        Debug.Log($"Member join: {_steamId.Name}");
       
    }

    private void SteamMatchmaking_OnLobbyEntered(Lobby _lobby)
    {
        Debug.Log("enter lobby ");
        currentLobby = _lobby;
        if (NetworkManager.Singleton.IsHost)
        {
            return;
        }

        StartClient(currentLobby.Value.Owner.Id);
    }

    private async void SteamMatchmaking_OnLobbyCreated(Result _result, Lobby _lobby)
    {
        if (_result != Result.OK)
        {
            Debug.Log("lobby was not created");
            return;
        }

        _lobby.SetPublic();
        _lobby.SetData("white", "white");
        _lobby.SetJoinable(true);
        _lobby.SetGameServer(_lobby.Owner.Id);
        Debug.Log($"lobby created by {_lobby.Owner.Name}");
        await ServiceLocator.Current.Get<UIService>().OpenPanel("Assets/Prefabs/LobbyPanel.prefab");
        
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<NetPlayer>().RequestAddMeToDictionaryServerRPC
            (SteamClient.SteamId,SteamClient.Name,NetworkManager.Singleton.LocalClientId);
    }

    public async void StartHost(int _maxMembers)
    {
        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
        NetworkManager.Singleton.StartHost();

        currentLobby = await SteamMatchmaking.CreateLobbyAsync(_maxMembers);
    }

    public void StartClient(SteamId _steamId)
    {
        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        transport.targetSteamId = _steamId;
        
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client has Started");
        }
        
    }

    public void DisConnected()
    {
        currentLobby?.Leave();
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.OnServerStarted -= Singleton_OnServerStarted;
        }
        else
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        }

        NetworkManager.Singleton.Shutdown(true);

        Debug.Log(" disconnected");
    }

    private void Singleton_OnClientDisconnectCallback(ulong obj)
    {
    }

    private async void Singleton_OnClientConnectedCallback(ulong obj)
    {
        Debug.Log("OnClientConnectedCallback");
        ServiceLocator.Current.Get<UIService>().ClosePanel();
        await ServiceLocator.Current.Get<UIService>().OpenPanel("Assets/Prefabs/LobbyPanel.prefab");
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<NetPlayer>().RequestAddMeToDictionaryServerRPC
            (SteamClient.SteamId,SteamClient.Name,NetworkManager.Singleton.LocalClientId);
    }

    private void Singleton_OnServerStarted()
    {
        Debug.Log("Host started");
    }
}
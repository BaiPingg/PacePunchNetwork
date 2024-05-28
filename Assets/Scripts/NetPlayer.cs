using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetPlayer : NetworkBehaviour
{
    [SerializeField] private AddPlayerToLobbyPlayerListsEvent _addPlayerToLobbyPlayerListsEvent;
    [SerializeField] private LobbyChatEvent _lobbyChatEvent;
    [SerializeField] private UpdateClientEvent _updateClientEvent;
    [SerializeField] private RemovePlayerToLobbyPlayerListsEvent _removePlayerToLobbyPlayerListsEvent;
    [SerializeField] private LobbyMemberReadySetEvent _lobbyMemberReadySetEvent;

    /// <summary>
    /// 请求服务器广播一条聊天信息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="formWho"></param>
    [ServerRpc(RequireOwnership = false)]
    public void RequestSendAChatServerRpc(string message, ulong formWho)
    {
        BroadcastAChatClientRpc(message, formWho);
    }

    /// <summary>
    /// 广播一条聊天信息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="fromWho"></param>
    [ClientRpc]
    public void BroadcastAChatClientRpc(string message, ulong fromWho)
    {
        _lobbyChatEvent.Raise(new LobbyChatMeassage()
        {
            message = message, clientIdForm = fromWho
        });
    }

    /// <summary>
    /// 请求服务器将我加入到玩家列表
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void RequestAddMeToDictionaryServerRPC(ulong steamId, string steamName, ulong clientId)
    {
        BroadcastAChatClientRpc($"{steamName} has joined", clientId);
        //发送事件 触发ui变化
        _addPlayerToLobbyPlayerListsEvent.Raise(new PlayerBaseInfo()
        {
            clientId = clientId, steamName = steamName, streamId = steamId
        });
        _updateClientEvent.Raise(null);
    }

    /// <summary>
    /// 同步已经存在的玩家给客户端
    /// </summary>
    /// <param name="steamId"></param>
    /// <param name="steamName"></param>
    /// <param name="clientId"></param>
    [ClientRpc]
    public void UpdateClientsPlayerInfoClientRPC(ulong steamId, string steamName, ulong clientId)
    {
        _addPlayerToLobbyPlayerListsEvent.Raise(new PlayerBaseInfo()
        {
            clientId = clientId, steamName = steamName, streamId = steamId
        });
    }

    /// <summary>
    /// 请求从玩家列表移除
    /// </summary>
    /// <param name="steamId"></param>
    [ServerRpc]
    public void RequestRemoveMeFormPlayerListsServerRpc(ulong steamId, ulong clientId, string steamName)
    {
        BroatcastRemovePlayerFromDictionaryClientRPC(steamId, clientId, steamName);
    }

    /// <summary>
    /// 广播玩家离开队列
    /// </summary>
    /// <param name="steamId"></param>
    [ClientRpc]
    public void BroatcastRemovePlayerFromDictionaryClientRPC(ulong steamId, ulong clientId, string steamName)
    {
        BroadcastAChatClientRpc($"{steamId} leave", clientId);
        _removePlayerToLobbyPlayerListsEvent.Raise(new PlayerBaseInfo()
        {
            clientId = clientId, steamName = steamName, streamId = steamId
        });
    }

    /// <summary>
    /// 请求设置准备状态
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="ready"></param>
    [ServerRpc]
    public void RequestSetMemberIsReadyServerRpc(ulong clientId, bool ready)
    {
        BroadcastSetMemberIsReadyClientRpc(clientId, ready);
    }

    [ClientRpc]
    public void BroadcastSetMemberIsReadyClientRpc(ulong clientId, bool ready)
    {
        _lobbyMemberReadySetEvent.Raise(new PlayerReadySet()
        {
            clientId = clientId,
            ready = ready,
        });
    }
   // [ServerRpc]
    
}
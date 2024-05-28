using UnityEngine;


/// <summary>
/// 将刚刚进入大厅的玩家加入到玩家列表
/// </summary>
[CreateAssetMenu(fileName = "RemovePlayerToLobbyPlayerListsEvent",
    menuName = "ScriptableObjects/RemovePlayerToLobbyPlayerListsEvent",
    order = 3)]
public class RemovePlayerToLobbyPlayerListsEvent : GameEvent<PlayerBaseInfo>
{
}
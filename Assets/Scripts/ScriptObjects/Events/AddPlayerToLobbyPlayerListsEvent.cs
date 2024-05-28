using UnityEngine;

public class PlayerBaseInfo
{
    public ulong streamId;
    public string steamName;
    public ulong clientId;
}
/// <summary>
/// 将刚刚进入大厅的玩家加入到玩家列表
/// </summary>
[CreateAssetMenu(fileName = "AddPlayerToLobbyPlayerListsEvent", menuName = "ScriptableObjects/AddPlayerToLobbyPlayerListsEvent",
    order = 3)]
public class AddPlayerToLobbyPlayerListsEvent : GameEvent<PlayerBaseInfo>
{
}
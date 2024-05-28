using UnityEngine;

public class LobbyChatMeassage
{
  
    public string message;
    public ulong clientIdForm;
}
/// <summary>
/// 将刚刚进入大厅的玩家加入到玩家列表
/// </summary>
[CreateAssetMenu(fileName = "LobbyChatEvent", menuName = "ScriptableObjects/LobbyChatEvent",
    order = 3)]
public class LobbyChatEvent : GameEvent<LobbyChatMeassage>
{
}
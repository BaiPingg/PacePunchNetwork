using UnityEngine;

public class PlayerReadySet
{
    public ulong clientId;
    public bool ready;
}

[CreateAssetMenu(fileName = "LobbyMemberReadySetEvent",
    menuName = "ScriptableObjects/LobbyMemberReadySetEvent",
    order = 3)]
public class LobbyMemberReadySetEvent : GameEvent<PlayerReadySet>
{
}
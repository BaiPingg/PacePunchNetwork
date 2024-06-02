using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyCard : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _currentPlayerCount;
    [SerializeField] private TMP_Text _ping;

    public void Init(CSteamID lobby)
    {
        gameObject.SetActive(true);

        _name.text = SteamMatchmaking.GetLobbyData(lobby, "Name");

        _currentPlayerCount.text =
            $"{SteamMatchmaking.GetNumLobbyMembers(lobby)}/{SteamMatchmaking.GetLobbyMemberLimit(lobby)}";
        _ping.text = "10ms";
    }

    private void OnDestroy()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
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
  
    public  void Init(Lobby lobby)
    {
   
        gameObject.SetActive(true);
        _name.text = lobby.Owner.Name;
        _currentPlayerCount.text = $"{lobby.MemberCount}/{lobby.MaxMembers}";
        _ping.text = "10ms";
       
    }

    private void OnDestroy()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }

   
}
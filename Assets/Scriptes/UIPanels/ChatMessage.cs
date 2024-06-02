using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class LobbyChatMeassage
{
  
    public string message;
    public ulong clientIdForm;
}
public class ChatMessage : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private TMP_Text _message;
    private ulong clientForm;

    public void Init(LobbyChatMeassage message)
    {
        transform.SetParent(_parent);
        gameObject.SetActive(true);
        clientForm = message.clientIdForm;
        _message.text = $"ClientId {clientForm}: {message.message}";
    }
}
using System;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;
 
    [SerializeField] private GameObject menuScreen, lobbyScreen;
    [SerializeField] private Button menuScreenFirstSelect, lobbyScreenFirstSelect;
    [SerializeField] private TMP_InputField lobbyInput;
 
    [SerializeField] private TextMeshProUGUI lobbyTitle, lobbyIDText;
    [SerializeField] private Button startGameButton;
    private void Awake() => instance = this;
 
    private void Start()
    {
        OpenMainMenu();
    }
 
    public void CreateLobby()
    {
        BootstrapManager.CreateLobby();
    }
 
    public void OpenMainMenu()
    {
        CloseAllScreens();
        menuScreen.SetActive(true);
        menuScreenFirstSelect.Select();
    }
 
    public void OpenLobby()
    {
        CloseAllScreens();
        lobbyScreen.SetActive(true);
        lobbyScreenFirstSelect.Select();
    }
 
    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        instance.lobbyTitle.text = lobbyName;
        instance.startGameButton.gameObject.SetActive(isHost);
        instance.lobbyIDText.text = BootstrapManager.CurrentLobbyID.ToString();
        instance.OpenLobby();
    }
 
    void CloseAllScreens()
    {
        menuScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }
 
    public void JoinLobby()
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        BootstrapManager.JoinByID(steamID);
        
    }
 
    public void LeaveLobby()
    {
        BootstrapManager.LeaveLobby();
        OpenMainMenu();
    }
 
   
}
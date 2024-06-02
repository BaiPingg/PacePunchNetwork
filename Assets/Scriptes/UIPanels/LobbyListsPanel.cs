using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListsPanel : UIPanel
{
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _joinBtn;

    [SerializeField] private Button _refreshBtn;
    [SerializeField] private GameObject _lobbyListContent;
    [SerializeField] private LobbyCard _lobbyCardTemplate;

    private CSteamID? currentLobby;

    private Callback<LobbyMatchList_t> LobbyMatchList;

    private void OnLobbyMatchList(LobbyMatchList_t list)
    {
        for (int i = 0; i < list.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyid = SteamMatchmaking.GetLobbyByIndex(i);

            var lobbyCard = Instantiate(_lobbyCardTemplate.gameObject, _lobbyListContent.transform)
                .GetComponent<LobbyCard>();
            lobbyCard.Init(lobbyid);
            lobbyCard.GetComponent<Button>().onClick.AddListener(() => { currentLobby = lobbyid; });
        }
    }

    public override void OnOpen(UIService service)
    {
        base.OnOpen(service);

        LobbyMatchList = LobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
        _backBtn.onClick.AddListener(BackToMainMenu);
        _joinBtn.onClick.AddListener(JoinLobby);
        _refreshBtn.onClick.AddListener(RefreshList);
    }


    public override void OnClose()
    {
        base.OnClose();
        _backBtn.onClick.RemoveAllListeners();
        _joinBtn.onClick.RemoveAllListeners();
        _refreshBtn.onClick.RemoveAllListeners();
    }


    private void RefreshList()
    {
        for (int i = 0; i < _lobbyListContent.transform.childCount; i++)
        {
            Destroy(_lobbyListContent.transform.GetChild(i).gameObject);
        }

        SL.Get<LobbyService>().FindLobbies();
    }

    private void JoinLobby()
    {
        if (currentLobby != null)
        {
            SL.Get<LobbyService>().JoinLobby(currentLobby.Value);
        }
    }

    private void BackToMainMenu()
    {
        SL.Get<UIService>().ClosePanel();
    }
}
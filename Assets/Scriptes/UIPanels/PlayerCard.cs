using System.Collections;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBaseInfo
{
    public ulong streamId;
    public string steamName;
    public ulong clientId;
}
public class PlayerCard : MonoBehaviour
{
    [SerializeField] private Sprite _defaultImage;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private GameObject _ready;
    [SerializeField] private Image _icon;
    public bool ready;
    public string steamName;
    public ulong steamId;
    public ulong cilentId;

    public async void Init(PlayerBaseInfo friend)
    {
        steamId = friend.streamId;
        steamName = friend.steamName;
        cilentId = friend.clientId;
        gameObject.SetActive(true);
        _name.text = steamName;
        // var image = await SteamFriends.GetLargeAvatarAsync(steamId);
        // if (!image.HasValue)
        //     _icon.sprite = _defaultImage;
        //
        // _icon.sprite = MakeTextureFromRGBA(image.Value.Data, image.Value.Width, image.Value.Height);
        // SetReady(false);
    }

    private Sprite MakeTextureFromRGBA(byte[] valueData, uint valueWidth, uint valueHeight)
    {
        var tex = new Texture2D((int)valueWidth, (int)valueHeight, TextureFormat.RGBA32,
            false, true);
        tex.LoadRawTextureData(valueData);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height)
            , new Vector2(0.5f, 0.5f), 100.0f);
    }

    public void SetReady(bool ready)
    {
        this.ready = ready;
        _ready.gameObject.SetActive(ready);
    }
}
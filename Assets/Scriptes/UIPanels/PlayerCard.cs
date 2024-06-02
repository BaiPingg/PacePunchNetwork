using System;
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
    [HideInInspector] public bool ready;
    [HideInInspector] public string steamName;
    [HideInInspector] public ulong steamId;
    [HideInInspector] public ulong cilentId;
    private Callback<AvatarImageLoaded_t> AvatarImageLoaded;

    private void OnEnable()
    {
        AvatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }

    private void OnAvatarImageLoaded(AvatarImageLoaded_t arg)
    {
     
    }


    public async void Init(PlayerBaseInfo friend)
    {
        steamId = friend.streamId;
        steamName = friend.steamName;
        cilentId = friend.clientId;
        gameObject.SetActive(true);
        _name.text = steamName;
        var image = SteamFriends.GetLargeFriendAvatar(new CSteamID(steamId));
        var tex = GetLargeAvatar(new CSteamID(steamId));
        _icon.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height)
            , new Vector2(0.5f, 0.5f), 100.0f);
        SetReady(false);
    }

    public Texture2D GetLargeAvatar(CSteamID user)
    {
        int FriendAvatar = SteamFriends.GetLargeFriendAvatar(user);
        uint ImageWidth;
        uint ImageHeight;
        bool success = SteamUtils.GetImageSize(FriendAvatar, out ImageWidth, out ImageHeight);

        if (success && ImageWidth > 0 && ImageHeight > 0)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];
            Texture2D returnTexture =
                new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
            success = SteamUtils.GetImageRGBA(FriendAvatar, Image, (int)(ImageWidth * ImageHeight * 4));
            if (success)
            {
                returnTexture.LoadRawTextureData(Image);
                returnTexture.Apply();
            }

            return returnTexture;
        }
        else
        {
            Debug.LogError("Couldn't get avatar.");
            return new Texture2D(0, 0);
        }
    }

    public void SetReady(bool ready)
    {
        this.ready = ready;
        _ready.gameObject.SetActive(ready);
    }
}
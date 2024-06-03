using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using UnityEngine;

public class LoadingGameState : State
{
    public UIPanel uiPanel;

    public override void Enter()
    {
        Debug.Log($"[{GetType().Name}]: enter");
        SL.Get<UIService>().CloseAllPanel();
        SL.Get<UIService>().OpenPanel(uiPanel);
        SL.Get<NetAddressable>().LoadAddressPackage("Assets/Prefabs/PlayerArmature.prefab");

        SceneLoadData sld = new SceneLoadData("SteamGameScene");
        NetworkConnection[] conns = InstanceFinder.ServerManager.Clients.Values.ToArray();
        InstanceFinder.SceneManager.LoadConnectionScenes(conns, sld);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
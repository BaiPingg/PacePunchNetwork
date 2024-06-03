using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadingGameState : State
{
  

    public override void Enter()
    {
        Debug.Log($"[{GetType().Name}]: enter");
        SL.Get<UIService>().CloseAllPanel();
        Addressables.InstantiateAsync(nameof(LoadingPanel)).Completed += handle =>
        {
            SL.Get<UIService>().OpenPanel(handle.Result.GetComponent<UIPanel>());
          
        };
       
      

        if (InstanceFinder.IsServerStarted)
        {
            SL.Get<NetAddressable>().LoadAddressPackage("Assets/Prefabs/PlayerArmature.prefab");
            SceneLoadData sld = new SceneLoadData("SteamGameScene");
            NetworkConnection[] conns = InstanceFinder.ServerManager.Clients.Values.ToArray();
            InstanceFinder.SceneManager.LoadConnectionScenes(conns, sld);
        }
        
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
using Newtonsoft.Json;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/StartStateAction")]
public class StartStateAction : Action
{
    [SerializeField] private LoadArchiveFinishedEvent loadArchiveFinishedEvent;

    public override async void Act(StateController controller)
    {
        var res = ServiceLocator.Current.Get<ResourcesSerive>();
        var op = res.LoadAssetAsync<GameObject>("Assets/Prefabs/NetPlayer.prefab");
        var prefab = await op.Task;
        NetworkManager.Singleton.AddNetworkPrefab(prefab);
        Debug.Log($"Loaded prefab has been assigned to NetworkManager's PlayerPrefab");
        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = prefab;
        NetworkManager.Singleton.NetworkConfig.ForceSamePrefabs = true;


        var target1 = await res.InstantiateAsync("Assets/Prefabs/PressAnyKeyPanel.prefab").Task;
        var panel = target1.GetComponent<UIPanel>();
        Debug.Log("load panel finish");
        ServiceLocator.Current.Get<UIService>().OpenPanel(panel);
        var fileService = ServiceLocator.Current.Get<FileService>();
        if (!fileService.FileExists(GameSetting.saveString))
        {
            fileService.WirteStringToFile(GameSetting.saveString, JsonConvert.SerializeObject(fileService.gameSetting));
        }
        else
        {
            var ss = fileService.LoadFileToString(GameSetting.saveString);
            fileService.gameSetting = JsonConvert.DeserializeObject<GameSetting>(ss);
        }

        loadArchiveFinishedEvent.Raise(null);
    }
}
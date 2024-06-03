using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;

public class LoadingPanel : UIPanel
{
    public override void OnOpen(UIService service)
    {
        base.OnOpen(service);
     
        InstanceFinder.SceneManager.OnLoadStart += OnLoadingSceneStart;
        InstanceFinder.SceneManager.OnLoadEnd += OnLoadingSceneEnd;
        InstanceFinder.SceneManager.OnLoadPercentChange += OnLoadingPercentChange;
    }

    private void OnLoadingPercentChange(SceneLoadPercentEventArgs obj)
    {
        Debug.Log($"percent :{obj.Percent}");
    }

    private void OnLoadingSceneEnd(SceneLoadEndEventArgs obj)
    {
        Debug.Log("load scene end");
        SL.Get<UIService>().ClosePanel();
        GameState gameState = new GameState();
        SL.Get<ProcedureService>().SwitchState(gameState);
    }

    private void OnLoadingSceneStart(SceneLoadStartEventArgs obj)
    {
        Debug.Log("load scene start");
    }

    public override void OnClose()
    {
        base.OnClose();
        InstanceFinder.SceneManager.OnLoadStart -= OnLoadingSceneStart;
        InstanceFinder.SceneManager.OnLoadEnd -= OnLoadingSceneEnd;
        InstanceFinder.SceneManager.OnLoadPercentChange -= OnLoadingPercentChange;
    }
}
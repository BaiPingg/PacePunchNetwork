using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PressAnyKeyPanel : UIPanel
{
    [SerializeField] private GameObject _pressAnyKey;

    public void OnLoadArchiveFinshed()
    {
        _pressAnyKey.gameObject.SetActive(true);
        InputSystem.onAnyButtonPress.CallOnce(OnAnyButtonPress);
    }

    private async void OnAnyButtonPress(InputControl ctrl)
    {
        Debug.Log($"Button {ctrl} was pressed, enter main menu");
        var res = ServiceLocator.Current.Get<ResourcesSerive>();
        var state = await res.LoadAssetAsync<State>("Assets/ScriptObjects/GameProcedure/MainMenuState.asset").Task;
        ServiceLocator.Current.Get<ProcedureService>().TransitionToState(state);
    }
}
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class MainMenuState : ProcedureStateBase
{
    public UIPanel uiPanel;

    public override void Enter()
    {
        Debug.Log($"[{GetType().Name}]:  enter");
        if (uiPanel != null)
        {
            SL.Get<UIService>().OpenPanel(uiPanel);
        }
    }


    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }

    public MainMenuState(StateMachine stateMachine) : base(stateMachine)
    {
    }
}
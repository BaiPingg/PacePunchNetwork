using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SplashState : ProcedureStateBase
{
    public override void Enter()
    {
        Debug.Log($"[{GetType().Name}]:  enter");

        Addressables.InstantiateAsync(nameof(MainMenuPanel)).Completed += handle =>
        {
            var state= new MainMenuState(_stateMachine);
            state.uiPanel = handle.Result.GetComponent<UIPanel>();
            _stateMachine.SwitchState(state);
           
        };
    }


    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }

    public SplashState(StateMachine stateMachine) : base(stateMachine)
    {
    }
}
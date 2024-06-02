using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SplashState : ProcedureStateBase
{
    public override void Enter()
    {
        Debug.Log($"[{GetType().Name}]: SplashState enter");
#if UNITY_EDITOR
        _stateMachine.StartCoroutine(PlaySplash());
#endif
        Addressables.InstantiateAsync(nameof(MainMenuPanel)).Completed += handle =>
        {
            SL.Get<UIService>().OpenPanel(handle.Result.GetComponent<UIPanel>());
        };
    }


    IEnumerator PlaySplash()
    {
        Debug.Log($"[{GetType().Name}]:Showing splash screen");
        SplashScreen.Begin();
        while (!SplashScreen.isFinished)
        {
            SplashScreen.Draw();
            yield return null;
        }

        Debug.Log($"[{GetType().Name}]:Finished showing splash screen");
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
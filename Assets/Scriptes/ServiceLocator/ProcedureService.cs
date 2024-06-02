using System;
using UnityEngine;


public class ProcedureService : StateMachine, IService
{
    private void Start()
    {
        var splashState = new SplashState(this);
        SwitchState(splashState);
    }
}
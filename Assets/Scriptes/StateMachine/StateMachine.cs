﻿using FishNet.Object;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State _currentState;

    public void SwitchState(State state)
    {
        _currentState?.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    private void Update()
    {
        _currentState?.Tick(Time.deltaTime);
    }
}
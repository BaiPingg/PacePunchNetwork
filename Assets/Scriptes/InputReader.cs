using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : NetworkBehaviour, MyInputActions.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;
    public Action OnJumpPerformed;

    private MyInputActions controls;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (controls != null)
            return;

        controls = new MyInputActions();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        controls.Player.Disable();
    }

 

    public void OnMove(InputAction.CallbackContext context)
    {
        if (IsOwner)
            MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsOwner)
        {
            if (!context.performed)
            {
                return;
            }

            OnJumpPerformed?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (IsOwner)
            MouseDelta = context.ReadValue<Vector2>();
    }
}
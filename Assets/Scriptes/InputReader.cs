using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, MyInputActions.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;
    public Action OnJumpPerformed;

    private MyInputActions controls;

    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new MyInputActions();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        OnJumpPerformed?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }
}
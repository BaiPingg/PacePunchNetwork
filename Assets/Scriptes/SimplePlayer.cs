using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayer : NetworkBehaviour
{
    Vector3 velocity = Vector3.zero;
  

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        velocity = new Vector3(value.x,0, value.y);
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime * 1.5f;
    }
}
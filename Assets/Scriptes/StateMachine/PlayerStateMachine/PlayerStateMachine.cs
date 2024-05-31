using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Vector3 velocity;
    public float moveSpeed;
    public float jumpForce ;
    public float lookRotationDampFactor { get; private set; } = 10f;
    public Transform mainCamera { get; private set; }
    public InputReader inputReader { get; private set; }
    public Animator animator{ get; private set; }
    public CharacterController controller { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main.transform;
        inputReader = GetComponent<InputReader>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(new PlayerMoveState(this));
    }
}


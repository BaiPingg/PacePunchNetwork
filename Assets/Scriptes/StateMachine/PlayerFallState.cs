using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");
    private const float CrossFadeDuration = 0.1f;

    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
       
        stateMachine.velocity.y = 0f;

        stateMachine.animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTim)
    {
       
        ApplyGravity();
        Move();

        if (stateMachine.controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }
    }

    public override void Exit()
    {
      
    }
}
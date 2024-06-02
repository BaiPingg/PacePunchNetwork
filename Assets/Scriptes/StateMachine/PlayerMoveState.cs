using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
    private const float AnimationDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public override void Enter()
    {
       
        stateMachine.velocity.y = Physics.gravity.y;
        stateMachine.animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);
        stateMachine.inputReader.OnJumpPerformed += SwitchToJumpState;
    }


    public override void Tick(float deltatime)
    {
       
        if (!stateMachine.controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        CalculateMoveDirection();
        FaceMoveDirection();
        Move();

        stateMachine.animator.SetFloat(MoveSpeedHash,
            stateMachine.inputReader.MoveComposite.sqrMagnitude > 0f ? 1f : 0f, AnimationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
      
        stateMachine.inputReader.OnJumpPerformed -= SwitchToJumpState;
    }

    private void SwitchToJumpState()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }


    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
}
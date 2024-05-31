using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine stateMachine;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void CalculateMoveDirection()
    {
        Vector3 cameraForward = new(stateMachine.mainCamera.forward.x, 0, stateMachine.mainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.mainCamera.right.x, 0, stateMachine.mainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * stateMachine.inputReader.MoveComposite.y +
                                cameraRight.normalized * stateMachine.inputReader.MoveComposite.x;

        stateMachine.velocity.x = moveDirection.x * stateMachine.moveSpeed;
        stateMachine.velocity.z = moveDirection.z * stateMachine.moveSpeed;
    }

    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(stateMachine.velocity.x, 0f, stateMachine.velocity.z);
        if (faceDirection == Vector3.zero)
            return;
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(faceDirection), stateMachine.lookRotationDampFactor * Time.deltaTime);
    }

    protected void ApplyGravity()
    {
        if (stateMachine.velocity.y > Physics.gravity.y)
        {
            stateMachine.velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    protected void Move()
    {
        stateMachine.controller.Move(stateMachine.velocity * Time.deltaTime);
    }
}
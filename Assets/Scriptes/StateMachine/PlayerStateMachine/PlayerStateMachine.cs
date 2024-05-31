using System;
using Cinemachine;
using FishNet;
using FishNet.Object;
using UnityEngine;

public class PlayerStateMachine : NetworkBehaviour
{
    public Vector3 velocity;
    public float moveSpeed;
    public float jumpForce;
    public float lookRotationDampFactor { get; private set; } = 10f;
    public Transform mainCamera { get; private set; }
    public InputReader inputReader { get; private set; }
    public Animator animator;
    public CharacterController controller { get; private set; }
    public GameObject cinemachineCameraTarget;
    public CinemachineFreeLook cinemachineFreeLook;

    private void Awake()
    {
        InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
        InstanceFinder.TimeManager.OnUpdate += TimeManager_OnUpdate;

        controller = GetComponent<CharacterController>();
    }


    private void OnDestroy()
    {
        if (InstanceFinder.TimeManager != null)
        {
            InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
            InstanceFinder.TimeManager.OnUpdate -= TimeManager_OnUpdate;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();


        controller.enabled = (base.IsServer || base.IsOwner);
        if (IsOwner)
        {
            mainCamera = Camera.main.transform;
            inputReader = GetComponent<InputReader>();
            inputReader.enabled = true;
            cinemachineFreeLook.gameObject.SetActive(true);
            cinemachineFreeLook.Follow = transform;
            cinemachineFreeLook.LookAt = cinemachineCameraTarget.transform;
            SwitchState(new PlayerMoveState(this));
        }
        else
        {
            cinemachineFreeLook.gameObject.SetActive(false);
        }
    }


    private void TimeManager_OnUpdate()
    {
        if (IsOwner)
        {
            _currentState?.Tick(Time.deltaTime);
        }
    }

    private void TimeManager_OnTick()
    {
    }

    #region StateMachine

    private State _currentState;

    public void SwitchState(State state)
    {
        _currentState?.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    #endregion
}
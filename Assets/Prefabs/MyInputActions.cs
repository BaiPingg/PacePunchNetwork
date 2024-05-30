//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Prefabs/MyInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MyInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MyInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MyInputActions"",
    ""maps"": [
        {
            ""name"": ""Player ControlMap"",
            ""id"": ""ae30f48e-c23e-49c5-b93d-6a986599e277"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""5b19ce87-1954-4d1d-abce-b4aa6a235fd1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5d841d4b-4eb3-424f-b622-ef3b8f581c25"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4418ec77-5ec8-4048-b08e-30d609790ed6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a83d4337-6bfd-45e1-b8b2-ddf16da0315d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b2b77993-2184-475e-b1a8-429358ba2822"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""29ddbf99-5531-44b9-a3f8-c904a3c99bd3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player ControlMap
        m_PlayerControlMap = asset.FindActionMap("Player ControlMap", throwIfNotFound: true);
        m_PlayerControlMap_Move = m_PlayerControlMap.FindAction("Move", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player ControlMap
    private readonly InputActionMap m_PlayerControlMap;
    private List<IPlayerControlMapActions> m_PlayerControlMapActionsCallbackInterfaces = new List<IPlayerControlMapActions>();
    private readonly InputAction m_PlayerControlMap_Move;
    public struct PlayerControlMapActions
    {
        private @MyInputActions m_Wrapper;
        public PlayerControlMapActions(@MyInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControlMap_Move;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControlMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlMapActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerControlMapActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerControlMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerControlMapActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
        }

        private void UnregisterCallbacks(IPlayerControlMapActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
        }

        public void RemoveCallbacks(IPlayerControlMapActions instance)
        {
            if (m_Wrapper.m_PlayerControlMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerControlMapActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerControlMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerControlMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerControlMapActions @PlayerControlMap => new PlayerControlMapActions(this);
    public interface IPlayerControlMapActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}

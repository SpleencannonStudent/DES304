// GENERATED AUTOMATICALLY FROM 'Assets/FPS_Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @FPS_Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @FPS_Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""FPS_Controls"",
    ""maps"": [
        {
            ""name"": ""Shooter"",
            ""id"": ""1bf04b05-68e7-4c01-9bdc-9ff67e26fd6b"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""1d368108-f5b9-4185-a1a6-7ffc9b513a4c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""549b3870-3083-4896-9d6a-3433eb5e6e30"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6812ef6b-9716-43ca-bb71-354012ffdb35"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AllowFire"",
                    ""type"": ""Button"",
                    ""id"": ""cfa5e054-f1ee-47cb-b3d3-4e130cae26a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""3a871add-eda9-49ba-9fb5-54faa8f56cd5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""572b2ff6-1f30-436f-b33d-fea97e10a51b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fc78ac00-772e-42fb-8a0f-77753066cffc"",
                    ""path"": ""<DualShockGamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Movement"",
                    ""id"": ""63272c71-91e2-4647-89c7-3942933bc708"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a73a2443-81a4-43b0-afa9-b9bb6cb55782"",
                    ""path"": ""<DualShockGamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""308d937c-4149-437c-9fb4-dd00470eef20"",
                    ""path"": ""<DualShockGamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a6c8e384-b840-42b6-9816-a9fbbfe398ef"",
                    ""path"": ""<DualShockGamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7953806b-af84-499d-b94f-e4918895e6e9"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Camera"",
                    ""id"": ""9e90368e-9fce-45c0-91d6-2c1ba7ff6e6d"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e22cc14d-661d-43e4-8ff6-77cc2b992ba2"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d1df4ef2-278f-46b9-b44c-fe119006d835"",
                    ""path"": ""<DualShockGamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""34f133a7-0e5d-45dc-b7c7-062abfc57600"",
                    ""path"": ""<DualShockGamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5d5571ae-2abf-4e22-ade4-db7adc4306f8"",
                    ""path"": ""<DualShockGamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7bcd7d78-f979-4787-aec9-852cc9ff88e2"",
                    ""path"": ""<DualShockGamepad>/rightTrigger"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AllowFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d8f6d65-fda8-4676-82ee-8e9376193a4d"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""004dcc76-ece0-47e9-b2c9-ec320a4c3b71"",
                    ""path"": ""<DualShockGamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Shooter
        m_Shooter = asset.FindActionMap("Shooter", throwIfNotFound: true);
        m_Shooter_Fire = m_Shooter.FindAction("Fire", throwIfNotFound: true);
        m_Shooter_Move = m_Shooter.FindAction("Move", throwIfNotFound: true);
        m_Shooter_Aim = m_Shooter.FindAction("Aim", throwIfNotFound: true);
        m_Shooter_AllowFire = m_Shooter.FindAction("AllowFire", throwIfNotFound: true);
        m_Shooter_Jump = m_Shooter.FindAction("Jump", throwIfNotFound: true);
        m_Shooter_Boost = m_Shooter.FindAction("Boost", throwIfNotFound: true);
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

    // Shooter
    private readonly InputActionMap m_Shooter;
    private IShooterActions m_ShooterActionsCallbackInterface;
    private readonly InputAction m_Shooter_Fire;
    private readonly InputAction m_Shooter_Move;
    private readonly InputAction m_Shooter_Aim;
    private readonly InputAction m_Shooter_AllowFire;
    private readonly InputAction m_Shooter_Jump;
    private readonly InputAction m_Shooter_Boost;
    public struct ShooterActions
    {
        private @FPS_Controls m_Wrapper;
        public ShooterActions(@FPS_Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Shooter_Fire;
        public InputAction @Move => m_Wrapper.m_Shooter_Move;
        public InputAction @Aim => m_Wrapper.m_Shooter_Aim;
        public InputAction @AllowFire => m_Wrapper.m_Shooter_AllowFire;
        public InputAction @Jump => m_Wrapper.m_Shooter_Jump;
        public InputAction @Boost => m_Wrapper.m_Shooter_Boost;
        public InputActionMap Get() { return m_Wrapper.m_Shooter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShooterActions set) { return set.Get(); }
        public void SetCallbacks(IShooterActions instance)
        {
            if (m_Wrapper.m_ShooterActionsCallbackInterface != null)
            {
                @Fire.started -= m_Wrapper.m_ShooterActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_ShooterActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_ShooterActionsCallbackInterface.OnFire;
                @Move.started -= m_Wrapper.m_ShooterActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_ShooterActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_ShooterActionsCallbackInterface.OnMove;
                @Aim.started -= m_Wrapper.m_ShooterActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_ShooterActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_ShooterActionsCallbackInterface.OnAim;
                @AllowFire.started -= m_Wrapper.m_ShooterActionsCallbackInterface.OnAllowFire;
                @AllowFire.performed -= m_Wrapper.m_ShooterActionsCallbackInterface.OnAllowFire;
                @AllowFire.canceled -= m_Wrapper.m_ShooterActionsCallbackInterface.OnAllowFire;
                @Jump.started -= m_Wrapper.m_ShooterActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_ShooterActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_ShooterActionsCallbackInterface.OnJump;
                @Boost.started -= m_Wrapper.m_ShooterActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_ShooterActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_ShooterActionsCallbackInterface.OnBoost;
            }
            m_Wrapper.m_ShooterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @AllowFire.started += instance.OnAllowFire;
                @AllowFire.performed += instance.OnAllowFire;
                @AllowFire.canceled += instance.OnAllowFire;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
            }
        }
    }
    public ShooterActions @Shooter => new ShooterActions(this);
    public interface IShooterActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnAllowFire(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
    }
}

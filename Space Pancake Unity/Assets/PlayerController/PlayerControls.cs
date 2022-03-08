//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/PlayerController/PlayerControls.inputactions
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

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Personnage"",
            ""id"": ""cd2c15a5-b4f2-45e8-a2ad-0554c8e188e8"",
            ""actions"": [
                {
                    ""name"": ""Sauter"",
                    ""type"": ""Button"",
                    ""id"": ""8552a9c2-3dd8-43c1-8b03-f7f9474de936"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""9380c04b-f2bf-47da-af48-3fccc39f1e72"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Serpe"",
                    ""type"": ""Button"",
                    ""id"": ""381c45a8-ff46-46ce-afba-5c76553b8292"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""534d8f65-0127-476e-a02f-588a9d1e5cc4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""6131159c-25d2-4ef2-a76a-d24a823c6a0b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""80114b94-9ecb-4097-b957-edbcb8b3df96"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sauter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b37f8ef1-8515-4325-94c5-94d4b7b481ed"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sauter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""385e8c61-f44b-4158-b004-a13207ba7e31"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd790f9d-16cb-4c73-a796-cc9949ed90e2"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1116775-08b5-47d0-a771-dd17e633c1b9"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b61d1a28-4ef9-4226-89a5-1ea74f930aef"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9c410b4-6a28-405f-9096-741e7c3334af"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Serpe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bd0f3be-67da-4709-b04c-ebe43418f434"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Serpe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d0a0718-d443-4708-b8fc-2d1f3e73e04c"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23731fab-c301-4dd7-a13b-0f5fa58b6d56"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2685a2d-a6b4-4f9c-aaa8-fb60ee14e972"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4eb1f1b-490f-4418-a2a6-faad91a70336"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Personnage
        m_Personnage = asset.FindActionMap("Personnage", throwIfNotFound: true);
        m_Personnage_Sauter = m_Personnage.FindAction("Sauter", throwIfNotFound: true);
        m_Personnage_Run = m_Personnage.FindAction("Run", throwIfNotFound: true);
        m_Personnage_Serpe = m_Personnage.FindAction("Serpe", throwIfNotFound: true);
        m_Personnage_MoveRight = m_Personnage.FindAction("MoveRight", throwIfNotFound: true);
        m_Personnage_MoveLeft = m_Personnage.FindAction("MoveLeft", throwIfNotFound: true);
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

    // Personnage
    private readonly InputActionMap m_Personnage;
    private IPersonnageActions m_PersonnageActionsCallbackInterface;
    private readonly InputAction m_Personnage_Sauter;
    private readonly InputAction m_Personnage_Run;
    private readonly InputAction m_Personnage_Serpe;
    private readonly InputAction m_Personnage_MoveRight;
    private readonly InputAction m_Personnage_MoveLeft;
    public struct PersonnageActions
    {
        private @PlayerControls m_Wrapper;
        public PersonnageActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Sauter => m_Wrapper.m_Personnage_Sauter;
        public InputAction @Run => m_Wrapper.m_Personnage_Run;
        public InputAction @Serpe => m_Wrapper.m_Personnage_Serpe;
        public InputAction @MoveRight => m_Wrapper.m_Personnage_MoveRight;
        public InputAction @MoveLeft => m_Wrapper.m_Personnage_MoveLeft;
        public InputActionMap Get() { return m_Wrapper.m_Personnage; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PersonnageActions set) { return set.Get(); }
        public void SetCallbacks(IPersonnageActions instance)
        {
            if (m_Wrapper.m_PersonnageActionsCallbackInterface != null)
            {
                @Sauter.started -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnSauter;
                @Sauter.performed -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnSauter;
                @Sauter.canceled -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnSauter;
                @Run.started -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnRun;
                @Serpe.started -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnSerpe;
                @Serpe.performed -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnSerpe;
                @Serpe.canceled -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnSerpe;
                @MoveRight.started -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnMoveRight;
                @MoveLeft.started -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.performed -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.canceled -= m_Wrapper.m_PersonnageActionsCallbackInterface.OnMoveLeft;
            }
            m_Wrapper.m_PersonnageActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Sauter.started += instance.OnSauter;
                @Sauter.performed += instance.OnSauter;
                @Sauter.canceled += instance.OnSauter;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @Serpe.started += instance.OnSerpe;
                @Serpe.performed += instance.OnSerpe;
                @Serpe.canceled += instance.OnSerpe;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @MoveLeft.started += instance.OnMoveLeft;
                @MoveLeft.performed += instance.OnMoveLeft;
                @MoveLeft.canceled += instance.OnMoveLeft;
            }
        }
    }
    public PersonnageActions @Personnage => new PersonnageActions(this);
    public interface IPersonnageActions
    {
        void OnSauter(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnSerpe(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnMoveLeft(InputAction.CallbackContext context);
    }
}
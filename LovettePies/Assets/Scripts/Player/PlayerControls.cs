//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/Player/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Restaurant Controls"",
            ""id"": ""b249361f-37e2-4326-b925-ff7c334e7aa6"",
            ""actions"": [
                {
                    ""name"": ""AreaNavigation"",
                    ""type"": ""Value"",
                    ""id"": ""29ededec-b4c7-4d88-8af7-cd9722b637a7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MainAction"",
                    ""type"": ""Button"",
                    ""id"": ""26fe1650-8105-46e0-ba0b-aadc461f8eae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f2cdbfa0-646b-4ed6-a36c-fdbc2c360a72"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD Controls"",
                    ""id"": ""3cd1726b-e2ad-4054-8bf4-fd668a564b5b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9c816352-5d50-4090-844c-6f793d3345ce"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b1047ab5-2e37-44aa-98db-a116569041c8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""af35ae4a-26fd-4087-88a1-02efce0bda8f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4caf464e-b231-4677-8728-58de0d794238"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Controls"",
                    ""id"": ""6655fa83-8a45-43a1-94d4-7993aeebcc1c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6c3a94cd-a585-418e-b0d1-0fc6cc886dbf"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bad6c67c-942a-40e3-8e68-b8e2b5cfa07f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""963d5708-52df-48b6-9455-445aba183acb"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d7e60012-b69b-4ff8-bad8-c789b58bfc6b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""07379d44-e0a0-4212-a7fe-99723c2821fc"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""D-Pad [Gamepad]"",
                    ""id"": ""16a4b4c2-07ed-483f-a60f-2ef5f50344c9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6a533c07-02cd-4bfb-8aa8-12b54ce488d8"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f56735a5-9c10-48e0-9963-8fbaf5fb4f0b"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4b2235b3-0a5c-4dd5-afb6-02e83b61e932"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4b0e8608-d7ea-46c7-a7e5-692c09355456"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1450f0e5-eb41-4fe8-bbd3-d0aaf72ebe00"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f655fcd-1999-46d6-abe2-23cf153e6628"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac8f3121-adf1-4ec9-b370-a6af3fac4c1a"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""571e9488-1e42-4a7b-af43-5406c3038edd"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1417dd3c-c35b-424b-815a-3a3356ae5bdd"",
                    ""path"": ""<SwitchProControllerHID>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d23a5db1-acae-4587-b9a8-e535de7e858a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00f6b518-b9f0-4650-88e7-9f1b6a649c65"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ff31ee2-68d5-4739-8cb3-a1f358f1d73a"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Barbershop Controls"",
            ""id"": ""afcbf1be-c7e7-4dbd-bf9c-28e3782ec883"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""31e8575e-0b42-4915-9008-0e6b7e60bc10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a69fe0e9-7131-437e-944b-f0a3e5e3ffea"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Conversation Controls"",
            ""id"": ""bfcd958b-2d5f-4303-8e01-41eb8c324a2e"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""c0be6aef-17b3-4fb2-b8c8-1c8d8ef12a57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""80bab939-4d1f-460b-9d21-09b37140232d"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Basic Minigame Controls"",
            ""id"": ""124e5c38-d908-459d-9423-fe52e12ce1ee"",
            ""actions"": [
                {
                    ""name"": ""Basic Press"",
                    ""type"": ""Button"",
                    ""id"": ""ce7a56f2-d876-4963-966b-032465e49996"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Controller Movement Left"",
                    ""type"": ""Value"",
                    ""id"": ""9d57f9ca-0374-43cd-8e75-c6ed7d72493f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Controller Movement Right"",
                    ""type"": ""Value"",
                    ""id"": ""6020b188-d222-47f2-8bca-1459e3bde38a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Quit Minigame"",
                    ""type"": ""Button"",
                    ""id"": ""330f69da-3ec4-497b-912e-5a5fe6c60d9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e01a37d5-c248-433d-9baf-c9050ec2a890"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Basic Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""213c70d2-bc45-494c-bd9a-9f01e12161f2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Basic Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59c81f76-75d0-4ac4-8f4f-f943860fff16"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Controller Movement Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fae0db2-7dca-453b-bf20-d9fb3290a208"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Controller Movement Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c26ca59-138e-4643-807b-4a6e6d291829"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit Minigame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cc91dae-0cc3-4017-a4e5-1df353af91e3"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit Minigame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Restaurant Controls
        m_RestaurantControls = asset.FindActionMap("Restaurant Controls", throwIfNotFound: true);
        m_RestaurantControls_AreaNavigation = m_RestaurantControls.FindAction("AreaNavigation", throwIfNotFound: true);
        m_RestaurantControls_MainAction = m_RestaurantControls.FindAction("MainAction", throwIfNotFound: true);
        m_RestaurantControls_Pause = m_RestaurantControls.FindAction("Pause", throwIfNotFound: true);
        // Barbershop Controls
        m_BarbershopControls = asset.FindActionMap("Barbershop Controls", throwIfNotFound: true);
        m_BarbershopControls_Newaction = m_BarbershopControls.FindAction("New action", throwIfNotFound: true);
        // Conversation Controls
        m_ConversationControls = asset.FindActionMap("Conversation Controls", throwIfNotFound: true);
        m_ConversationControls_Newaction = m_ConversationControls.FindAction("New action", throwIfNotFound: true);
        // Basic Minigame Controls
        m_BasicMinigameControls = asset.FindActionMap("Basic Minigame Controls", throwIfNotFound: true);
        m_BasicMinigameControls_BasicPress = m_BasicMinigameControls.FindAction("Basic Press", throwIfNotFound: true);
        m_BasicMinigameControls_ControllerMovementLeft = m_BasicMinigameControls.FindAction("Controller Movement Left", throwIfNotFound: true);
        m_BasicMinigameControls_ControllerMovementRight = m_BasicMinigameControls.FindAction("Controller Movement Right", throwIfNotFound: true);
        m_BasicMinigameControls_QuitMinigame = m_BasicMinigameControls.FindAction("Quit Minigame", throwIfNotFound: true);
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

    // Restaurant Controls
    private readonly InputActionMap m_RestaurantControls;
    private List<IRestaurantControlsActions> m_RestaurantControlsActionsCallbackInterfaces = new List<IRestaurantControlsActions>();
    private readonly InputAction m_RestaurantControls_AreaNavigation;
    private readonly InputAction m_RestaurantControls_MainAction;
    private readonly InputAction m_RestaurantControls_Pause;
    public struct RestaurantControlsActions
    {
        private @PlayerControls m_Wrapper;
        public RestaurantControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @AreaNavigation => m_Wrapper.m_RestaurantControls_AreaNavigation;
        public InputAction @MainAction => m_Wrapper.m_RestaurantControls_MainAction;
        public InputAction @Pause => m_Wrapper.m_RestaurantControls_Pause;
        public InputActionMap Get() { return m_Wrapper.m_RestaurantControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RestaurantControlsActions set) { return set.Get(); }
        public void AddCallbacks(IRestaurantControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_RestaurantControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RestaurantControlsActionsCallbackInterfaces.Add(instance);
            @AreaNavigation.started += instance.OnAreaNavigation;
            @AreaNavigation.performed += instance.OnAreaNavigation;
            @AreaNavigation.canceled += instance.OnAreaNavigation;
            @MainAction.started += instance.OnMainAction;
            @MainAction.performed += instance.OnMainAction;
            @MainAction.canceled += instance.OnMainAction;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IRestaurantControlsActions instance)
        {
            @AreaNavigation.started -= instance.OnAreaNavigation;
            @AreaNavigation.performed -= instance.OnAreaNavigation;
            @AreaNavigation.canceled -= instance.OnAreaNavigation;
            @MainAction.started -= instance.OnMainAction;
            @MainAction.performed -= instance.OnMainAction;
            @MainAction.canceled -= instance.OnMainAction;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IRestaurantControlsActions instance)
        {
            if (m_Wrapper.m_RestaurantControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IRestaurantControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_RestaurantControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RestaurantControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public RestaurantControlsActions @RestaurantControls => new RestaurantControlsActions(this);

    // Barbershop Controls
    private readonly InputActionMap m_BarbershopControls;
    private List<IBarbershopControlsActions> m_BarbershopControlsActionsCallbackInterfaces = new List<IBarbershopControlsActions>();
    private readonly InputAction m_BarbershopControls_Newaction;
    public struct BarbershopControlsActions
    {
        private @PlayerControls m_Wrapper;
        public BarbershopControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_BarbershopControls_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_BarbershopControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BarbershopControlsActions set) { return set.Get(); }
        public void AddCallbacks(IBarbershopControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_BarbershopControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BarbershopControlsActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        private void UnregisterCallbacks(IBarbershopControlsActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        public void RemoveCallbacks(IBarbershopControlsActions instance)
        {
            if (m_Wrapper.m_BarbershopControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBarbershopControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_BarbershopControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BarbershopControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BarbershopControlsActions @BarbershopControls => new BarbershopControlsActions(this);

    // Conversation Controls
    private readonly InputActionMap m_ConversationControls;
    private List<IConversationControlsActions> m_ConversationControlsActionsCallbackInterfaces = new List<IConversationControlsActions>();
    private readonly InputAction m_ConversationControls_Newaction;
    public struct ConversationControlsActions
    {
        private @PlayerControls m_Wrapper;
        public ConversationControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_ConversationControls_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_ConversationControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ConversationControlsActions set) { return set.Get(); }
        public void AddCallbacks(IConversationControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_ConversationControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ConversationControlsActionsCallbackInterfaces.Add(instance);
            @Newaction.started += instance.OnNewaction;
            @Newaction.performed += instance.OnNewaction;
            @Newaction.canceled += instance.OnNewaction;
        }

        private void UnregisterCallbacks(IConversationControlsActions instance)
        {
            @Newaction.started -= instance.OnNewaction;
            @Newaction.performed -= instance.OnNewaction;
            @Newaction.canceled -= instance.OnNewaction;
        }

        public void RemoveCallbacks(IConversationControlsActions instance)
        {
            if (m_Wrapper.m_ConversationControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IConversationControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_ConversationControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ConversationControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ConversationControlsActions @ConversationControls => new ConversationControlsActions(this);

    // Basic Minigame Controls
    private readonly InputActionMap m_BasicMinigameControls;
    private List<IBasicMinigameControlsActions> m_BasicMinigameControlsActionsCallbackInterfaces = new List<IBasicMinigameControlsActions>();
    private readonly InputAction m_BasicMinigameControls_BasicPress;
    private readonly InputAction m_BasicMinigameControls_ControllerMovementLeft;
    private readonly InputAction m_BasicMinigameControls_ControllerMovementRight;
    private readonly InputAction m_BasicMinigameControls_QuitMinigame;
    public struct BasicMinigameControlsActions
    {
        private @PlayerControls m_Wrapper;
        public BasicMinigameControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @BasicPress => m_Wrapper.m_BasicMinigameControls_BasicPress;
        public InputAction @ControllerMovementLeft => m_Wrapper.m_BasicMinigameControls_ControllerMovementLeft;
        public InputAction @ControllerMovementRight => m_Wrapper.m_BasicMinigameControls_ControllerMovementRight;
        public InputAction @QuitMinigame => m_Wrapper.m_BasicMinigameControls_QuitMinigame;
        public InputActionMap Get() { return m_Wrapper.m_BasicMinigameControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicMinigameControlsActions set) { return set.Get(); }
        public void AddCallbacks(IBasicMinigameControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_BasicMinigameControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BasicMinigameControlsActionsCallbackInterfaces.Add(instance);
            @BasicPress.started += instance.OnBasicPress;
            @BasicPress.performed += instance.OnBasicPress;
            @BasicPress.canceled += instance.OnBasicPress;
            @ControllerMovementLeft.started += instance.OnControllerMovementLeft;
            @ControllerMovementLeft.performed += instance.OnControllerMovementLeft;
            @ControllerMovementLeft.canceled += instance.OnControllerMovementLeft;
            @ControllerMovementRight.started += instance.OnControllerMovementRight;
            @ControllerMovementRight.performed += instance.OnControllerMovementRight;
            @ControllerMovementRight.canceled += instance.OnControllerMovementRight;
            @QuitMinigame.started += instance.OnQuitMinigame;
            @QuitMinigame.performed += instance.OnQuitMinigame;
            @QuitMinigame.canceled += instance.OnQuitMinigame;
        }

        private void UnregisterCallbacks(IBasicMinigameControlsActions instance)
        {
            @BasicPress.started -= instance.OnBasicPress;
            @BasicPress.performed -= instance.OnBasicPress;
            @BasicPress.canceled -= instance.OnBasicPress;
            @ControllerMovementLeft.started -= instance.OnControllerMovementLeft;
            @ControllerMovementLeft.performed -= instance.OnControllerMovementLeft;
            @ControllerMovementLeft.canceled -= instance.OnControllerMovementLeft;
            @ControllerMovementRight.started -= instance.OnControllerMovementRight;
            @ControllerMovementRight.performed -= instance.OnControllerMovementRight;
            @ControllerMovementRight.canceled -= instance.OnControllerMovementRight;
            @QuitMinigame.started -= instance.OnQuitMinigame;
            @QuitMinigame.performed -= instance.OnQuitMinigame;
            @QuitMinigame.canceled -= instance.OnQuitMinigame;
        }

        public void RemoveCallbacks(IBasicMinigameControlsActions instance)
        {
            if (m_Wrapper.m_BasicMinigameControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBasicMinigameControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_BasicMinigameControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BasicMinigameControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BasicMinigameControlsActions @BasicMinigameControls => new BasicMinigameControlsActions(this);
    public interface IRestaurantControlsActions
    {
        void OnAreaNavigation(InputAction.CallbackContext context);
        void OnMainAction(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IBarbershopControlsActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IConversationControlsActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IBasicMinigameControlsActions
    {
        void OnBasicPress(InputAction.CallbackContext context);
        void OnControllerMovementLeft(InputAction.CallbackContext context);
        void OnControllerMovementRight(InputAction.CallbackContext context);
        void OnQuitMinigame(InputAction.CallbackContext context);
    }
}

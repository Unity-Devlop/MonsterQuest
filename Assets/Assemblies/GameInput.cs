//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.1
//     from Assets/Settings/GameInput.inputactions
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
using UnityEngine;

namespace Game
{
    public partial class @GameInput: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""38ea77ef-9232-4edb-ae5b-c874485afdd3"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""4876ecca-de63-4036-811f-4b1f659356a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""66826012-7fa4-41b2-9b59-3c1a45d7dae1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""597f135e-6161-44e2-b162-974a55b1d3dd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""3e7f0376-87e6-40dd-bd6e-54e753579e3d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""3f2a29f8-a994-4d4e-be62-9c8be0892f0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenChatPanel"",
                    ""type"": ""Button"",
                    ""id"": ""4f96b89b-9ae5-4487-bc30-a20e6f33dbf4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e7b6e21f-8045-470a-82d7-d8b7d3087615"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b5d2186-7df4-4543-af79-45cfbed03b00"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d33001e3-322e-4f4d-84e7-6331ab48c2d6"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse;Touch"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79446bc2-9a64-4f2e-8c38-773e1cf414d5"",
                    ""path"": ""<Joystick>/{Hatswitch}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6eb4d7e-55a0-42e5-a27c-bbe9a4d61ae0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""ddcbc8b6-dc03-4634-a1ff-7e576c6f7aab"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0ee84103-46d3-45e1-91ca-b633e3b4ffe7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2b62554d-bdc2-40fe-97e1-3db45524c0d5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""adf61a82-4dcb-4139-b20f-c469f7c68d55"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1aef7ac2-75c3-409a-8015-06f3d2c7142d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ad784251-630f-46f8-a7b0-c8d664f77ec3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6ebc5070-4c76-46dd-8924-b015a01e31c4"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6c61303f-f03b-4ca8-ae8a-f7a9420ff8bc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a9450f69-dbe2-4d39-bcd5-048f4301ac0a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""321f9f99-c494-4bd3-932c-acbcd32d2f71"",
                    ""path"": ""<XRController>/{Primary2DAxis}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52683198-7559-4072-b08f-e7b50f4f726d"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6f636d1-4c8e-4949-ae2d-0a0c697664d4"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42b5c250-7625-478b-a65a-73a735ecc474"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f593952f-c95f-407f-916d-ac4cd874b3c6"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Touch"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14e56043-6fe1-4653-b6fa-9f978a6e3249"",
                    ""path"": ""<Joystick>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29aaf1cb-7986-478b-8ce5-0929ad640ce4"",
                    ""path"": ""<XRController>/{PrimaryAction}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b481144-7f08-47d0-9acf-afb8cdadd099"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87b2ab7a-7460-4b89-b9c7-e57656957abd"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78bfc3cb-68ba-4f2e-b663-1bb877eb0d46"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenChatPanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""f340886e-5b52-487a-a0a9-a97a140dd3d8"",
            ""actions"": [
                {
                    ""name"": ""OpenPackage"",
                    ""type"": ""Button"",
                    ""id"": ""bc184443-1953-46d9-a211-b162a349a581"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenShop"",
                    ""type"": ""Button"",
                    ""id"": ""c068a303-c088-4264-9b5b-6b9244863153"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CloseTop"",
                    ""type"": ""Button"",
                    ""id"": ""ad87319e-3d0f-446c-946d-9438334355be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7a0ab6de-56fb-44c1-b690-156b27070223"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenShop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8046cf8-b03d-407b-88bd-b78deb9896a4"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenPackage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""719bbc74-e40b-4c85-b9ca-c71c2de42530"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
            m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
            m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
            m_Player_OpenChatPanel = m_Player.FindAction("OpenChatPanel", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_OpenPackage = m_UI.FindAction("OpenPackage", throwIfNotFound: true);
            m_UI_OpenShop = m_UI.FindAction("OpenShop", throwIfNotFound: true);
            m_UI_CloseTop = m_UI.FindAction("CloseTop", throwIfNotFound: true);
        }

        ~@GameInput()
        {
            Debug.Assert(!m_Player.enabled, "This will cause a leak and performance issues, GameInput.Player.Disable() has not been called.");
            Debug.Assert(!m_UI.enabled, "This will cause a leak and performance issues, GameInput.UI.Disable() has not been called.");
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

        // Player
        private readonly InputActionMap m_Player;
        private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
        private readonly InputAction m_Player_Interact;
        private readonly InputAction m_Player_Look;
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_Fire;
        private readonly InputAction m_Player_Run;
        private readonly InputAction m_Player_OpenChatPanel;
        public struct PlayerActions
        {
            private @GameInput m_Wrapper;
            public PlayerActions(@GameInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Interact => m_Wrapper.m_Player_Interact;
            public InputAction @Look => m_Wrapper.m_Player_Look;
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @Fire => m_Wrapper.m_Player_Fire;
            public InputAction @Run => m_Wrapper.m_Player_Run;
            public InputAction @OpenChatPanel => m_Wrapper.m_Player_OpenChatPanel;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @OpenChatPanel.started += instance.OnOpenChatPanel;
                @OpenChatPanel.performed += instance.OnOpenChatPanel;
                @OpenChatPanel.canceled += instance.OnOpenChatPanel;
            }

            private void UnregisterCallbacks(IPlayerActions instance)
            {
                @Interact.started -= instance.OnInteract;
                @Interact.performed -= instance.OnInteract;
                @Interact.canceled -= instance.OnInteract;
                @Look.started -= instance.OnLook;
                @Look.performed -= instance.OnLook;
                @Look.canceled -= instance.OnLook;
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Fire.started -= instance.OnFire;
                @Fire.performed -= instance.OnFire;
                @Fire.canceled -= instance.OnFire;
                @Run.started -= instance.OnRun;
                @Run.performed -= instance.OnRun;
                @Run.canceled -= instance.OnRun;
                @OpenChatPanel.started -= instance.OnOpenChatPanel;
                @OpenChatPanel.performed -= instance.OnOpenChatPanel;
                @OpenChatPanel.canceled -= instance.OnOpenChatPanel;
            }

            public void RemoveCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerActions @Player => new PlayerActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
        private readonly InputAction m_UI_OpenPackage;
        private readonly InputAction m_UI_OpenShop;
        private readonly InputAction m_UI_CloseTop;
        public struct UIActions
        {
            private @GameInput m_Wrapper;
            public UIActions(@GameInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @OpenPackage => m_Wrapper.m_UI_OpenPackage;
            public InputAction @OpenShop => m_Wrapper.m_UI_OpenShop;
            public InputAction @CloseTop => m_Wrapper.m_UI_CloseTop;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void AddCallbacks(IUIActions instance)
            {
                if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
                @OpenPackage.started += instance.OnOpenPackage;
                @OpenPackage.performed += instance.OnOpenPackage;
                @OpenPackage.canceled += instance.OnOpenPackage;
                @OpenShop.started += instance.OnOpenShop;
                @OpenShop.performed += instance.OnOpenShop;
                @OpenShop.canceled += instance.OnOpenShop;
                @CloseTop.started += instance.OnCloseTop;
                @CloseTop.performed += instance.OnCloseTop;
                @CloseTop.canceled += instance.OnCloseTop;
            }

            private void UnregisterCallbacks(IUIActions instance)
            {
                @OpenPackage.started -= instance.OnOpenPackage;
                @OpenPackage.performed -= instance.OnOpenPackage;
                @OpenPackage.canceled -= instance.OnOpenPackage;
                @OpenShop.started -= instance.OnOpenShop;
                @OpenShop.performed -= instance.OnOpenShop;
                @OpenShop.canceled -= instance.OnOpenShop;
                @CloseTop.started -= instance.OnCloseTop;
                @CloseTop.performed -= instance.OnCloseTop;
                @CloseTop.canceled -= instance.OnCloseTop;
            }

            public void RemoveCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IUIActions instance)
            {
                foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public UIActions @UI => new UIActions(this);
        public interface IPlayerActions
        {
            void OnInteract(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnFire(InputAction.CallbackContext context);
            void OnRun(InputAction.CallbackContext context);
            void OnOpenChatPanel(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnOpenPackage(InputAction.CallbackContext context);
            void OnOpenShop(InputAction.CallbackContext context);
            void OnCloseTop(InputAction.CallbackContext context);
        }
    }
}

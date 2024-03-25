using System;
using System.Collections.Generic;
using Cinemachine;
using Game.UI;
using MemoryPack;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game
{
    public class Player : NetworkBehaviour
    {
        public enum NetworkState
        {
            WaitFofSpawnPokemon,
            Ready,
        }

        public static Player Local { get; private set; }
        public CinemachineFreeLook freeLookCamera { get; private set; }

        public static event Action OnLocalPlayerSpawned;
        private CinemachineInputProvider _cameraInputProvider;

        public PlayerController controller { get; private set; }

        private string playerName => data.playerName;
        public string userId => data.userId;

        [field: SerializeField] public PlayerData data { get; private set; }

        [field: SerializeField] public PackageData package { get; private set; }

        [field: SerializeField] public NetworkState state { get; private set; }
        
        public GameInput.PlayerActions input => InputManager.Singleton.input.Player;

        private void Awake()
        {
            freeLookCamera = transform.Find("Camera").GetComponent<CinemachineFreeLook>();
            _cameraInputProvider = freeLookCamera.GetComponent<CinemachineInputProvider>();
        }

        public override void OnStartServer()
        {
            PokemonServer.Singleton.AddPlayer(this);
        }

        public override void OnStopServer()
        {
            if (PokemonServer.SingletonNullable == null) return;
            PokemonServer.Singleton.RemovePlayer(this);
            // 保持位置信息
            PokemonServer.Singleton.UpdatePosition(userId, controller.position);
        }

        public override void OnStartClient()
        {
            freeLookCamera.enabled = isLocalPlayer;


            if (isLocalPlayer)
            {
                Local = this;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (isClientOnly) // 非Host下的Client Host下的数据 已经在ServerInitData中初始化
            {
                CmdInitData(); // 所有客户端都要请求初始化数据
            }

            if (isServer && isClient) // Host
            {
                HostInitData();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!isLocalPlayer) return;
            Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !hasFocus;
        }

        public override void OnStopClient()
        {
            // if (PokemonClient.SingletonNullable == null) return;
            if (isLocalPlayer)
            {
                Local = null;
            }
        }

        public void DisableInput()
        {
            input.Disable();
            DisableCursor();
        }

        public void EnableInput()
        {
            input.Enable();
            EnableCursor();
        }

        private void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cameraInputProvider.enabled = false;
        }

        private void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cameraInputProvider.enabled = true;
        }

        private void Update()
        {
            if (isLocalPlayer && state == NetworkState.Ready)
            {
                TickFrameStateLogic();
            }
        }

        private void FixedUpdate()
        {
            if (isLocalPlayer && state == NetworkState.Ready)
            {
                TickPhysicsStateLogic();
            }
        }


        private void TickFrameStateLogic()
        {
            if (input.Fire.WasPressedThisFrame() && Cursor.lockState == CursorLockMode.Locked)
            {
                controller.HandleAttack();
            }

            // Cursor Logic
            if (Cursor.lockState == CursorLockMode.Locked) // 指针锁定时
            {
                if (Keyboard.current.altKey.wasPressedThisFrame)
                {
                    DisableCursor();
                }
            }
            else if (Cursor.lockState == CursorLockMode.None && UIRoot.Singleton.CurTop() is GamePanel) // 指针没有锁定时
            {
                if (Keyboard.current.altKey.wasReleasedThisFrame) // 松开alt键 则锁定
                {
                    EnableCursor();
                }
            }


            if (Keyboard.current.bKey.wasPressedThisFrame)
            {
                UIRoot.Singleton.OpenPanel<PackagePanel>();
            }
            else if (input.OpenChatPanel.WasPressedThisFrame())
            {
                bool success = UIRoot.Singleton.GetOpenedPanel(out GamePanel panel);
                Assert.IsTrue(success);
                panel.OpenChatPanel();
            }
            else if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (UIRoot.Singleton.CurTop() is not GamePanel)
                {
                    UIRoot.Singleton.CloseTop();
                }
                else
                {
                    if (UIRoot.Singleton.CurTop() is GamePanel gamePanel)
                    {
                        gamePanel.CloseSub();
                    }
                }
            }

            if (Keyboard.current.backquoteKey.wasPressedThisFrame)
            {
                if (DebugPanel.Singleton.IsOpen())
                {
                    DebugPanel.Singleton.Close();
                }
                else
                {
                    DebugPanel.Singleton.Open();
                }
            }
        }

        private void TickPhysicsStateLogic()
        {
            // 必须要结束瞬时状态才能切换到其他状态
            Vector2 moveInput = input.Move.ReadValue<Vector2>();
            if (moveInput.sqrMagnitude > 0.01f)
            {
                Vector3 pokemonPos = controller.transform.position;
                Vector3 cameraPos = freeLookCamera.transform.position;
                Vector3 viewDir = pokemonPos - new Vector3(cameraPos.x, pokemonPos.y, cameraPos.z);
                if (input.Run.IsPressed())
                {
                    controller.HandleRun(viewDir, moveInput);
                }
                else
                {
                    controller.HandleWalk(viewDir, moveInput);
                }
            }
            else
            {
                controller.HandleIdle();
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdInitData(NetworkConnectionToClient sender = null)
        {
            // 根据userId查数据
            PokemonServer.Singleton.QueryPlayerData(userId, out var playerData);
            PokemonServer.Singleton.QueryPackageData(userId, out var packageData);
            ArraySegment<byte> playerDataPayload = MemoryPackSerializer.Serialize(playerData);
            // if (isLocalPlayer)
            // {
            ArraySegment<byte> packageDataPayload = MemoryPackSerializer.Serialize(packageData); // TODO 只有本地玩家才需要背包数据
            // 通知指定的客户端初始化数据
            TargetInitData(sender, playerDataPayload, packageDataPayload, controller.netIdentity,
                controller.position);
            // }
            // else
            // {
            //     TargetInitData(sender, playerDataPayload, ArraySegment<byte>.Empty, pokemonController.pokemonIdentity,
            //         pokemonController.pokemonPosition);
            // }
        }


        [TargetRpc]
        private void TargetInitData(NetworkConnectionToClient conn, ArraySegment<byte> playerDataPayload,
            ArraySegment<byte> packageDataPayload, NetworkIdentity pokemon, Vector3 position)
        {
            data = MemoryPackSerializer.Deserialize<PlayerData>(playerDataPayload);
            package = MemoryPackSerializer.Deserialize<PackageData>(packageDataPayload); // TODO 只有本地玩家才需要背包数据
            gameObject.name = $"Player:[{playerName}]";
            // 配置信息
            GameObject pokemonObj = pokemon.gameObject;
            pokemonObj.name = $"{playerName}-Pokemon:[{data.self.configId}]";
            PlayerSetup(pokemonObj, position);

            Debug.Log($"Init Data: {data.playerName}");
            // 玩家初始化完成
            if (isLocalPlayer)
            {
                // Debug.Log("Local Player Spawned");
                OnLocalPlayerSpawned?.Invoke();
            }
        }

        private void HostInitData()
        {
            if (isLocalPlayer)
            {
                OnLocalPlayerSpawned?.Invoke();
            }
        }

        [Server]
        public void ServerInitData(PlayerData playerData, PackageData packageData, NetworkConnectionToClient connection,
            Vector3 position)
        {
            data = playerData;
            package = packageData;
            PokemonEnum pokemonId = data.self.configId;
            GameObject prefab = GlobalManager.Singleton.configTable.playerEntityPrefab;
            GameObject obj = Instantiate(prefab, null);
            obj.name = $"{playerName}]-Pokemon:[{pokemonId}]";
            PlayerSetup(obj, position);
            NetworkServer.Spawn(obj, connection);
        }

        private void PlayerSetup(GameObject obj, Vector3 position)
        {
            controller = obj.GetComponent<PlayerController>();
            controller.Init(this, data.self, position);
            Transform modelTransform = controller.transform;
            freeLookCamera.Follow = modelTransform;
            freeLookCamera.LookAt = modelTransform;
            state = NetworkState.Ready;
        }


        [Command]
        private void CmdChangeGroup(int id)
        {
            PokemonServer.Singleton.QueryGroupData(id, out TeamGroup groupData);
            RpcChangeGroup(MemoryPackSerializer.Serialize(groupData));
        }

        [ClientRpc]
        private void RpcChangeGroup(ArraySegment<byte> groupPayload)
        {
            TeamGroup groupData = MemoryPackSerializer.Deserialize<TeamGroup>(groupPayload);
            data.groupId = groupData.id; // todo 
        }


        [Command(requiresAuthority = false)]
        public void CmdSendChatMessage(ArraySegment<byte> payload)
        {
            RpcReceiveChatMessage(payload);
        }

        [ClientRpc]
        private void RpcReceiveChatMessage(ArraySegment<byte> payload)
        {
            ChatMessage msg = MemoryPackSerializer.Deserialize<ChatMessage>(payload);
            GlobalManager.EventSystem.Send(msg);
            // Debug.Log($"Receive Chat Message: {msg.content}");
        }


        [TargetRpc]
        public void TargetAddScore(NetworkConnection conn, int i)
        {
            Debug.Log($"Add Score: {i}");
        }

        public void HandleAddItem(ItemEnum id, int number)
        {
            if (!isLocalPlayer && !isServer)
            {
                Debug.LogError("HandleAddItem: Not Local Player");
                return;
            }

            CmdAddItem(id, number);
        }

        [Command]
        private void CmdAddItem(ItemEnum id, int number)
        {
            bool s1 = PokemonServer.Singleton.QueryPackageData(userId, out var data1);
            Assert.IsTrue(s1);
            Assert.IsTrue(data1 == package);
            Debug.Log(data1 == package);

            Debug.Log($"CmdAddItem: {id} {number}");
            package.AddItem(id, number);
            RpcAddItem(id, number);
        }

        [ClientRpc]
        private void RpcAddItem(ItemEnum id, int number)
        {
            if (isLocalPlayer && !isServer)
            {
                Debug.Log($"RpcAddItem: {id} {number}");
                package.AddItem(id, number);
            }
        }
    }
}
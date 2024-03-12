﻿using System;
using System.Collections.Generic;
using Cinemachine;
using Game.UI;
using MemoryPack;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game
{
    public enum PlayerState
    {
        WaitFofSpawnPokemon,
        Ready,
    }

    public class PlayerController : NetworkBehaviour
    {
        public static PlayerController LocalPlayer { get; private set; }
        private CinemachineFreeLook _camera;
        public Action OnSwitchPokemon;

        public static event Action OnLocalPlayerSpawned;
        // private CinemachineInputProvider _cameraInputProvider;

        public PokemonController pokemonController { get; private set; }
        
        // public Transform Orientation { get; private set; }
        public State<PokemonController> pokemonState => pokemonController.stateMachine.CurrentState;
        private string playerName => data.playerName;
        public string userId => data.userId;

        [field: SerializeField] public PlayerData data { get; private set; }

        [field: SerializeField] public PackageData package { get; private set; }

        [field: SerializeField] public PlayerState state { get; private set; }
        

        private GameInput.PlayerActions input => InputManager.Singleton.input.Player;

        private void Awake()
        {
            _camera = transform.Find("Camera").GetComponent<CinemachineFreeLook>();
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
            PokemonServer.Singleton.UpdatePosition(userId, pokemonController.pokemonPosition);
        }

        public override void OnStartClient()
        {
            _camera.enabled = isLocalPlayer;
            if (isClientOnly) // 非Host下的Client Host下的数据 已经在ServerInitData中初始化
            {
                CmdInitData(isLocalPlayer); // 所有客户端都要请求初始化数据
            }

            if (isLocalPlayer)
            {
                LocalPlayer = this;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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
                LocalPlayer = null;
            }
        }

        private void Update()
        {
            if (isLocalPlayer && state == PlayerState.Ready)
            {
                TickPokemonStateLogic();
                TickInputLogic();
            }
        }

        private void TickInputLogic()
        {

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Keyboard.current.bKey.wasPressedThisFrame)
            {
                if (!UIRoot.Singleton.IsOpen<PackagePanel>())
                {
                    UIRoot.Singleton.OpenPanel<PackagePanel>();
                }
                else
                {
                    UIRoot.Singleton.ClosePanel<PackagePanel>();
                }
            }
        }

        private void TickPokemonStateLogic()
        {
            // TODO 移动到PokemonController
            if (pokemonState is ITempAnimState { canExit: false })
            {
                return;
            }

            if (input.Fire.WasPressedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
            {
                pokemonController.stateMachine.Change<PokemonAttackState>(); // 本地立刻切换状态 避免异常
                pokemonController.CmdAttack();
                return;
            }


            // 必须要结束瞬时状态才能切换到其他状态

            Vector2 moveInput = input.Move.ReadValue<Vector2>();
            if (moveInput.sqrMagnitude > 0.01f)
            {
                Vector3 pokemonPos = pokemonController.pokemonTransform.position;
                Vector3 cameraPos = _camera.transform.position;
                Vector3 viewDir = pokemonPos - new Vector3(cameraPos.x, pokemonPos.y, cameraPos.z);
                if (input.Run.IsPressed())
                {
                    pokemonController.HandleRun(viewDir, moveInput);
                }
                else
                {
                    pokemonController.HandleWalk(viewDir, moveInput);
                }
            }
            else
            {
                pokemonController.HandleIdle();
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdInitData(bool isLocalPlayer, NetworkConnectionToClient sender = null)
        {
            // 根据userId查数据
            PokemonServer.Singleton.QueryPlayerData(userId, out var playerData);
            PokemonServer.Singleton.QueryPackageData(userId, out var packageData);
            ArraySegment<byte> playerDataPayload = MemoryPackSerializer.Serialize(playerData);
            // if (isLocalPlayer)
            // {
            ArraySegment<byte> packageDataPayload = MemoryPackSerializer.Serialize(packageData); // TODO 只有本地玩家才需要背包数据
            // 通知指定的客户端初始化数据
            TargetInitData(sender, playerDataPayload, packageDataPayload, pokemonController.pokemonIdentity,
                pokemonController.pokemonPosition);
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
            pokemonObj.name = $"{playerName}-Pokemon:[{data.currentPokemonData.configId}]";
            PokemonSetup(pokemonObj, position);

            // 玩家初始化完成
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
            PokemonEnum pokemonId = data.currentPokemonData.configId;
            GameObject prefab = GlobalManager.Singleton.configTable.GetPokemonConfig(pokemonId).prefab;
            GameObject pokemon = Instantiate(prefab, null);
            pokemon.name = $"{playerName}]-Pokemon:[{pokemonId}]";
            PokemonSetup(pokemon, position);
            NetworkServer.Spawn(pokemon, connection);
        }

        private void PokemonSetup(GameObject pokemon, Vector3 position)
        {
            pokemonController = pokemon.GetComponent<PokemonController>();
            pokemonController.InitPokemon(this, pokemon, data.currentPokemonData, position);
            Transform modelTransform = pokemonController.pokemonTransform;
            _camera.Follow = modelTransform;
            _camera.LookAt = modelTransform;
            state = PlayerState.Ready;
        }

        [Command]
        public void CmdSwitchPokemon()
        {
            throw new NotImplementedException();
        }

        public void HandleChangeGroup(int id)
        {
            CmdChangeGroup(id);
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
            data.group = groupData;
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
        }
        
        
        [TargetRpc]
        public void TargetAddScore(NetworkConnection conn, int i)
        {
            Debug.Log($"Add Score: {i}");
        }
    }
}
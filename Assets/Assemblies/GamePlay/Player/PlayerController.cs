﻿using System;
using Cinemachine;
using MemoryPack;
using Mirror;
using UnityEngine;
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
        private CinemachineFreeLook _camera;
        private CinemachineInputProvider _cameraInputProvider;

        public PokemonController pokemonController { get; private set; }
        public State<PokemonController> pokemonState => pokemonController.stateMachine.CurrentState;
        private string playerName => _data.playerName;
        public string userId => _data.userId;
        [SerializeField] private PlayerData _data;
        [SerializeField] private PackageData _packageData;

        [field: SerializeField] public PlayerState state { get; private set; }


        [SyncVar] public float facingAngle;

        private GameInput.PlayerActions input => InputManager.Singleton.input.Player;

        private void Awake()
        {
            pokemonController = GetComponent<PokemonController>();
            _camera = transform.Find("Camera").GetComponent<CinemachineFreeLook>();
            _cameraInputProvider = _camera.GetComponent<CinemachineInputProvider>();
        }

        public override void OnStartServer()
        {
            PokemonServer.Singleton.AddPlayer(this);
        }

        public override void OnStopServer()
        {
            PokemonServer.Singleton.RemovePlayer(this);
            // 保持位置信息
            PokemonServer.Singleton.UpdatePosition(userId, pokemonController.pokemonPosition);
        }

        public override void OnStartClient()
        {
            _camera.enabled = isLocalPlayer;
            if (!isServer)
            {
                CmdInitData(); // 所有客户端都要请求初始化数据
            }
        }

        private void Update()
        {
            if (isLocalPlayer && state == PlayerState.Ready)
            {
                TickPokemonStateLogic();
                if (Mouse.current.leftButton.isPressed)
                {
                    _cameraInputProvider.enabled = true;
                }
                else
                {
                    _cameraInputProvider.enabled = false;
                }
            }
        }

        private void TickPokemonStateLogic()
        {
            Vector2 moveInput = input.Move.ReadValue<Vector2>();
            if (moveInput.sqrMagnitude > 0.01f)
            {
                float forward = moveInput.y;
                float right = moveInput.x;
                // forward控制前后移动
                Vector3 moveVec = pokemonController.pokemonTransform.forward * forward;
                moveVec *= _data.moveSpeed * Time.deltaTime;
                //right控制旋转
                if (right != 0)
                {
                    // 插值
                    float deltaAngel = right * Time.deltaTime * Mathf.Rad2Deg;
                    pokemonController.CmdRotate(deltaAngel);
                }
pokemonController.stateMachine.Change<PokemonWalkState>();
                pokemonController.CmdWalk(moveVec);
            }
            else
            {
                pokemonController.stateMachine.Change<PokemonIdleState>();
                pokemonController.CmdIdle();
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdInitData(NetworkConnectionToClient sender = null)
        {
            // 根据userId查数据
            PokemonServer.Singleton.QueryPlayerData(userId, out var playerData);
            PokemonServer.Singleton.QueryPackageData(userId, out var packageData);
            ArraySegment<byte> playerDataPayload = MemoryPackSerializer.Serialize(playerData);
            ArraySegment<byte> packageDataPayload = MemoryPackSerializer.Serialize(packageData);
            // 通知指定的客户端初始化数据
            TargetInitData(sender, playerDataPayload, packageDataPayload, pokemonController.pokemonIdentity,
                pokemonController.pokemonPosition);
        }

        [TargetRpc]
        private void TargetInitData(NetworkConnectionToClient conn, ArraySegment<byte> playerDataPayload,
            ArraySegment<byte> packageDataPayload, NetworkIdentity pokemon, Vector3 position)
        {
            _data = MemoryPackSerializer.Deserialize<PlayerData>(playerDataPayload);
            _packageData = MemoryPackSerializer.Deserialize<PackageData>(packageDataPayload);
            gameObject.name = $"Player:[{playerName}]";
            // 配置信息
            pokemonController.InitPokemon(pokemon.gameObject, position);
            GameObject pokemonObj = pokemon.gameObject;
            pokemonObj.name = $"{playerName}-Pokemon:[{_data.currentPokemonId}]";
            
            PokemonSetup(pokemonObj, position);
        }

        [Server]
        public void ServerInitData(PlayerData playerData, PackageData packageData, NetworkConnectionToClient connection)
        {
            _data = playerData;
            _packageData = packageData;
            SpawnCurPokemon(_data.currentPokemonId, connection);
        }

        [Server]
        private void SpawnCurPokemon(int pokemonId, NetworkConnectionToClient connection)
        {
            PokemonServer.Singleton.QueryPosition(userId, out Vector3 position);
            PrefabTable.Instance.GetPokemonPrefab(pokemonId, out GameObject prefab);
            GameObject pokemon = Instantiate(prefab, null);
            pokemon.name = $"{playerName}]-Pokemon:[{pokemonId}]";
            NetworkServer.Spawn(pokemon, connection);
            PokemonSetup(pokemon, position);
        }

        private void PokemonSetup(GameObject pokemon, Vector3 position)
        {
            pokemonController.InitPokemon(pokemon.gameObject, position);
            Transform modelTransform = pokemonController.pokemonTransform;
            _camera.Follow = modelTransform;
            _camera.LookAt = modelTransform;
            state = PlayerState.Ready;
        }
    }
}
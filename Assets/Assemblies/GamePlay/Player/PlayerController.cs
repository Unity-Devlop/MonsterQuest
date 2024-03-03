using System;
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
        public Transform Orientation { get; private set; }
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
            Orientation = transform.Find("Orientation");
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

            if (isLocalPlayer)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public override void OnStopClient()
        {
            if (isLocalPlayer)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void Update()
        {
            if (isLocalPlayer && state == PlayerState.Ready)
            {
                TickPokemonStateLogic();
            }
        }

        private void TickPokemonStateLogic()
        {
            Vector2 moveInput = input.Move.ReadValue<Vector2>();
            if (moveInput.sqrMagnitude > 0.01f)
            {
                Vector3 pokemonPos = pokemonController.pokemonTransform.position;
                Vector3 cameraPos = _camera.transform.position;
                Vector3 viewDir = pokemonPos - new Vector3(cameraPos.x, pokemonPos.y, cameraPos.z);
                pokemonController.HandleWalk(viewDir, moveInput, _data.currentPokemonData.moveSpeed,
                    _data.currentPokemonData.rotateSpeed);
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
            pokemonObj.name = $"{playerName}-Pokemon:[{_data.currentPokemonData.configId}]";

            PokemonSetup(pokemonObj, position);
        }

        [Server]
        public void ServerInitData(PlayerData playerData, PackageData packageData, NetworkConnectionToClient connection)
        {
            _data = playerData;
            _packageData = packageData;
            SpawnCurPokemon(_data.currentPokemonData.configId, connection);
        }

        [Server]
        private void SpawnCurPokemon(int pokemonId, NetworkConnectionToClient connection)
        {
            PokemonServer.Singleton.QueryPosition(userId, out Vector3 position);
            GameObject prefab = ConfigTable.Instance.GetPokemonConfig(pokemonId).prefab;
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
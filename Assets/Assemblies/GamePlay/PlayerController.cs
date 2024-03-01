using System;
using Cinemachine;
using MemoryPack;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public enum PlayerState
    {
        WaitFofSpawnPokemon,
        Ready,
    }

    public class PlayerController : NetworkBehaviour
    {
        private CinemachineVirtualCamera _camera;

        public PokemonController pokemonController { get; private set; }


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
            _camera = transform.Find("Camera").GetComponent<CinemachineVirtualCamera>();
        }

        public override void OnStartClient()
        {
            _camera.enabled = isLocalPlayer;
            CmdInitData();
        }

        private void Update()
        {
            if (isLocalPlayer && state == PlayerState.Ready)
            {
                Vector2 moveDir = input.Move.ReadValue<Vector2>();
                if (moveDir.sqrMagnitude > 0.01f)
                {
                    Vector3 moveVec = moveDir * (_data.moveSpeed * Time.deltaTime);
                    pokemonController.CmdWalk(moveVec);
                }
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
            TargetInitData(sender, playerDataPayload, packageDataPayload);
        }

        [TargetRpc]
        private void TargetInitData(NetworkConnectionToClient conn, ArraySegment<byte> playerDataPayload,
            ArraySegment<byte> packageDataPayload)
        {
            _data = MemoryPackSerializer.Deserialize<PlayerData>(playerDataPayload);
            _packageData = MemoryPackSerializer.Deserialize<PackageData>(packageDataPayload);

            gameObject.name = $"Player:[{playerName}]";


            CmdSpawnCurPokemon(_data.currentPokemonId);
        }

        [Command(requiresAuthority = false)]
        private void CmdSpawnCurPokemon(int pokemonId, NetworkConnectionToClient sender = null)
        {
            PokemonServer.Singleton.QueryPosition(userId, out Vector3 position);
            TargetSpawnCurPokemon(sender, pokemonId, position);
        }

        [TargetRpc]
        private void TargetSpawnCurPokemon(NetworkConnectionToClient sender, int pokemonId, Vector3 position)
        {
            PrefabTable.Instance.GetPokemonPrefab(pokemonId, out GameObject prefab);
            GameObject pokemon = Instantiate(prefab, transform);
            pokemonController.Init(pokemon, position); // todo 后面加入的玩家不会执行这个 导致 NullReferenceException
            state = PlayerState.Ready;
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


        [Server]
        public void ServerInitData(PlayerData playerData, PackageData packageData)
        {
            _data = playerData;
            _packageData = packageData;
        }
    }
}
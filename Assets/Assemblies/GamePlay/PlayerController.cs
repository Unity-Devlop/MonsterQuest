using System;
using Cinemachine;
using MemoryPack;
using Mirror;
using UnityEngine;

namespace Game
{
    public class PlayerController : NetworkBehaviour
    {
        private CinemachineVirtualCamera _camera;
        private PokemonController _pokemonController;


        private string userName => _data.userName;
        private string userId => _data.userId;
        private PlayerData _data;
        private PackageData _packageData;


        [SyncVar] public float facingAngle;

        private void Awake()
        {
            _pokemonController = GetComponent<PokemonController>();
            _camera = transform.Find("Camera").GetComponent<CinemachineVirtualCamera>();
        }

        public override void OnStartClient()
        {
            _camera.enabled = isLocalPlayer;
            CmdInitData(Authentication.userId);
        }


        [Command]
        private void CmdInitData(string userId)
        {
            // 根据userId查数据
            PokemonServer.Singleton.QueryPlayerData(userId, out var playerData);
            PokemonServer.Singleton.QueryPackageData(userId, out var packageData);
            ArraySegment<byte> playerDataPayload = MemoryPackSerializer.Serialize(playerData);
            ArraySegment<byte> packageDataPayload = MemoryPackSerializer.Serialize(packageData);
            RpcInitData(playerDataPayload, packageDataPayload);
        }

        [ClientRpc]
        private void RpcInitData(ArraySegment<byte> playerDataPayload,
            ArraySegment<byte> packageDataPayload)
        {
            _data = MemoryPackSerializer.Deserialize<PlayerData>(playerDataPayload);
            _packageData = MemoryPackSerializer.Deserialize<PackageData>(packageDataPayload);
            CmdSpawnCurPokemon(_data.currentPokemonId);
        }

        [Command]
        private void CmdSpawnCurPokemon(int pokemonId)
        {
            RpcSpawnCurPokemon(pokemonId);
        }

        [ClientRpc]
        private void RpcSpawnCurPokemon(int pokemonId)
        {
            PrefabTable.Instance.GetPokemonPrefab(pokemonId, out GameObject prefab);
            GameObject pokemon = Instantiate(prefab, transform);
            _pokemonController.InitComponent(pokemon);
        }
    }
}
using System;
using MemoryPack;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    public enum ElementType
    {
        草,
        水,
        火
    }
    [Serializable]
    public struct PokemonConfig
    {
        public int id;
        public string name;
        public ElementType element;
        public float baseHitPoint;
        public float baseMaxHealth;
        public float baseMoveSpeed;
        public float baseRunSpeed;
        public float baseRotateSpeed;
        public float baseFlySpeed;
        [MemoryPackIgnore, JsonIgnore] public GameObject prefab;
    }

    [MemoryPackable, Serializable]
    public partial class PokemonData
    {
        public Guid uniqueId;
        public int configId;
        [MemoryPackIgnore, JsonIgnore] private bool _configInitialized;
        [MemoryPackIgnore, JsonIgnore] private PokemonConfig _config;

        [MemoryPackIgnore,JsonIgnore]
        public PokemonConfig config
        {
            get
            {
                if (!_configInitialized)
                {
                    _config = ConfigTable.Instance.GetPokemonConfig(configId);
                    _configInitialized = true;
                }

                return _config;
            }
        }

        public float moveSpeed;
        public float runSpeed;
        public float rotateSpeed;
        public float flySpeed;

        public PokemonData(int configId)
        {
            uniqueId = Guid.NewGuid();
            this.configId = configId;
            moveSpeed = config.baseMoveSpeed;
            runSpeed = config.baseRunSpeed;
            rotateSpeed = config.baseRotateSpeed;
            flySpeed = config.baseFlySpeed;
        }
    }
}
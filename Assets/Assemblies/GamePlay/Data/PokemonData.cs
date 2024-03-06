using System;
using MemoryPack;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    // [Serializable]
    // public struct HitBoxFrame
    // {
    //     public float time;
    //     public Vector3 center;
    //     public float radius;
    // }

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
        public int baseDamagePoint;
        public int baseMaxHealth;
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

        [MemoryPackIgnore, JsonIgnore]
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

        public int maxHealth;
        public int currentHealth;
        public int damagePoint;
        public float moveSpeed;
        public float runSpeed;
        public float rotateSpeed;
        public float flySpeed;

        public PokemonData(int configId)
        {
            uniqueId = Guid.NewGuid();
            this.configId = configId;
            maxHealth = config.baseMaxHealth;
            currentHealth = maxHealth;
            damagePoint = config.baseDamagePoint;
            moveSpeed = config.baseMoveSpeed;
            runSpeed = config.baseRunSpeed;
            rotateSpeed = config.baseRotateSpeed;
            flySpeed = config.baseFlySpeed;
        }
    }
}
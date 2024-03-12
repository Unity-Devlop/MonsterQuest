using System;
using MemoryPack;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{



    [MemoryPackable, Serializable]
    public partial class PokemonData
    {
        public Guid uniqueId;
        public PokemonEnum configId;
        [MemoryPackIgnore, JsonIgnore] private bool _configInitialized;
        [MemoryPackIgnore, JsonIgnore] private PokemonConfig _config;

        [MemoryPackIgnore, JsonIgnore]
        public PokemonConfig config
        {
            get
            {
                if (!_configInitialized)
                {
                    _config =  GlobalManager.Singleton.configTable.GetPokemonConfig(configId);
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

        public PokemonData(PokemonEnum configId)
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
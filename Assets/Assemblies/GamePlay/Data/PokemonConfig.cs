using System;
using MemoryPack;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct PokemonConfig
    {
        public int id;
        public string name => ((PokemonEnum)id).ToString();
        public ElementEnum element;
        public int baseDamagePoint;
        public int baseMaxHealth;
        public float baseMoveSpeed;
        public float baseRunSpeed;
        public float baseRotateSpeed;
        public float baseFlySpeed;
        // TODO 进化

        [MemoryPackIgnore, JsonIgnore] public GameObject prefab;
    }
}
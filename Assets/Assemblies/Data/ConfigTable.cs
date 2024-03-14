using System;
using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    // [GlobalConfig("Assets/AddressablesResources/DataTable/")]
    [CreateAssetMenu(fileName = "ConfigTable", menuName = "Game/ConfigTable")]
    public class ConfigTable : ScriptableObject
    {
        public SerializableDictionary<ItemEnum, ItemConfig> itemConfigs;
        public SerializableDictionary<PokemonEnum, PokemonConfig> pokemonConfigs;
        public GameObject playerEntityPrefab;

        public ItemConfig GetItemConfig(ItemEnum id)
        {
            return itemConfigs[id];
        }

        public PokemonConfig GetPokemonConfig(PokemonEnum configId)
        {
            return pokemonConfigs[configId];
        }
    }
}
using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [GlobalConfig("Assets/AddressablesResources/DataTable/"),
     CreateAssetMenu(fileName = "ConfigTable", menuName = "Game/ConfigTable")]
    public class ConfigTable : GlobalConfig<ConfigTable>
    {
        public SerializableDictionary<int, ItemConfig> itemConfigs;
        public SerializableDictionary<int,PokemonConfig> pokemonConfigs;

        public ItemConfig GetItemConfig(int id)
        {
            return itemConfigs[id];
        }

        public PokemonConfig GetPokemonConfig(int configId)
        {
            return pokemonConfigs[configId];
        }
    }
}
using System;
using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [GlobalConfig("Assets/AddressablesResources/DataTable/")]
    [CreateAssetMenu(fileName = "ConfigTable", menuName = "Game/ConfigTable")]
    public class ConfigTable : GlobalConfig<ConfigTable>
    {
        // private static ConfigTable _instance;
        //
        // public static ConfigTable Instance
        // {
        //     get
        //     {
        //         if (_instance == null)
        //         {
        //             _instance = Resources.Load<ConfigTable>("ConfigTable");// TODO Using Addressable
        //         }
        //         return _instance;
        //     }
        // }
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        // private static void ResetSingleton()
        // {
        //     _instance = null;
        // }


        public SerializableDictionary<ItemEnum, ItemConfig> itemConfigs;
        public SerializableDictionary<PokemonEnum, PokemonConfig> pokemonConfigs;

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
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game
{
    [CreateAssetMenu(fileName = "PrefabTable", menuName = "Game/PrefabTable")]
    [GlobalConfig("Assets/AddressablesResources/DataTable/")]
    public class PrefabTable : GlobalConfig<PrefabTable>
    {
        public SerializableDictionary<int, GameObject> pokemonPrefab;

        public void GetPokemonPrefab(int pokemonId, out GameObject gameObject)
        {
            gameObject = pokemonPrefab[pokemonId];
        }
    }
}
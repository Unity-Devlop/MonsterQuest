using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [RequireComponent(typeof(PokemonController))]
    public class WildPokemon : NetworkBehaviour
    {
        private PokemonController _pokemonController;

        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private PokemonData data;

        public PokemonEnum pokemonEnum;


        private void Awake()
        {
            _pokemonController = GetComponent<PokemonController>();
        }

        public override void OnStartClient()
        {
            data = new PokemonData(pokemonEnum);
            _pokemonController.Init(null, data, transform.position);
        }
    }
}
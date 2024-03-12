using System;
using MemoryPack;

namespace Game
{
    [MemoryPackable, Serializable]
    public partial class PlayerData
    {
        public string userId;
        public string playerName;

        public TeamGroup group;

        // pokemonData
        public PokemonData currentPokemonData;

        public PlayerData(string userId, string playerName, PokemonData currentPokemonData)
        {
            group = TeamGroup.Default;
            this.userId = userId;
            this.playerName = playerName;
            this.currentPokemonData = currentPokemonData;
        }
    }
}
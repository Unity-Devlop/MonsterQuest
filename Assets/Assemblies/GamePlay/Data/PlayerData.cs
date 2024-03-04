using System;
using MemoryPack;

namespace Game
{
    [MemoryPackable,Serializable]
    public partial class PlayerData
    {
        public string userId;
        public string playerName;
        // pokemonData
        public PokemonData currentPokemonData;
        
        public PlayerData(string userId,string playerName,PokemonData currentPokemonData)
        {
            this.userId = userId;
            this.playerName = playerName;
            this.currentPokemonData = currentPokemonData;
        }

    }
}
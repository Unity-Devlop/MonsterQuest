using System;
using MemoryPack;

namespace Game
{
    [MemoryPackable,Serializable]
    public partial class PlayerData
    {
        public string userId;
        public string playerName;
        public int level;
        
        // pokemonData
        public PokemonData currentPokemonData;

    }
}
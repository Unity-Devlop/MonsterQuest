using UnityToolkit;

namespace Game
{
    public struct OnLocalPlayerHealthChange
    {
        public int currentHealth;
        public int maxHealth;
    }

    public struct OnLocalPlayerLevelChange
    {
        public int currentLevel;
    }
}
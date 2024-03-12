using MemoryPack;

namespace Game
{
    [MemoryPackable]
    public partial struct ChatMessage 
    {
        public string uid;
        public string name;
        public string content;
    }
}
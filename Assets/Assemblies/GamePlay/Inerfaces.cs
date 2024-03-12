using Mirror;

namespace Game
{
    public delegate void OnHealthChangeDelegate(int current, int max);

    public interface IEntity
    {
        int groupId { get; }
    }

    public interface IHittable : IEntity
    {
        bool canBeHit { get; set; }
        void CmdBeAttack(int damagePoint, NetworkConnectionToClient sender = null);
    }

    public interface ITempAnimState
    {
        bool canExit { get; }
    }
}
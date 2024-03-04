using Mirror;

namespace Game
{
    public interface IEntity
    {
        int groupId { get; }
    }

    public interface IHittable : IEntity
    {
        bool canBeHit { get; }
        void CmdBeAttack(int damagePoint);
    }

    public interface ITempAnimState
    {
        bool canExit { get; }
    }
}
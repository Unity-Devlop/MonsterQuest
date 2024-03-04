
using Mirror;

namespace Game
{
    public interface IHittable
    {
        void CmdBeAttack(int damagePoint);
    }

    public interface ITempAnimState
    {
        bool canExit { get; }
    }
}
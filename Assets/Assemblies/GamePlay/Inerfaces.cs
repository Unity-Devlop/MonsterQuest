
using Mirror;

namespace Game
{
    public interface IHittable
    {
        int GroupId();
        void CmdBeAttack(int damagePoint);
    }

    public interface ITempAnimState
    {
        bool canExit { get; }
    }
}
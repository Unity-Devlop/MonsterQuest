
namespace Game
{
    public interface IHittable
    {
        bool CanBeHit();

        void CmdBeAttack(int damagePoint);
    }

    public interface ITempAnimState
    {
        bool canExit { get; }
    }
}
using UnityToolkit;

namespace Game
{
    public class PlayerWalkState : IState<PlayerController>
    {
        public void OnEnter(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.walk, true);
        }

        public void OnUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.walk, false);
        }
    }
}
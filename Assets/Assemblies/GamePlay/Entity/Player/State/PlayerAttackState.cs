using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PlayerAttackState : IState<PlayerController>, ITempAnimState
    {
        public void OnEnter(PlayerController owner)
        {
            over = false;
            GameObject grassAttack = Object.Instantiate(GlobalManager.Singleton.configTable.grassAttackPrefab);
            grassAttack.GetComponent<GrassAttack>().Setup(owner);
            Timer.Register(0.4f, () => { over = true; });
        }

        public void OnUpdate(PlayerController owner)
        {   
            AnimatorStateInfo stateInfo = owner.animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Attack")) return;
            if (stateInfo.normalizedTime > 0.95f)
            {
                over = true;
            }
        }

        public void OnExit(PlayerController owner)
        {
        }

        public bool canExit => over;
        public bool over { get; private set; }
    }
}
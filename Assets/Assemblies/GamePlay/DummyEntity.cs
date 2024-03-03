using Mirror;
using UnityEngine;

namespace Game
{
    public class DummyEntity : NetworkBehaviour,IHittable
    {
        public bool CanBeHit()
        {
            return true;
        }

        public void CmdBeAttack(int damagePoint)
        {
            Debug.Log($"CmdBeAttack:{damagePoint}");
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HitController : MonoBehaviour
    {
        protected readonly HashSet<Collider> filter = new HashSet<Collider>(10);

        public void Reset()
        {
            filter.Clear();
        }

        public virtual void Tick(PokemonController owner)
        {
        }
    }
}
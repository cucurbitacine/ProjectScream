using UnityEngine;

namespace Game.Scripts.Core
{
    public interface IBin
    {
        public bool CanDrop(GameObject target);
        public void Drop(GameObject target);
    }
}
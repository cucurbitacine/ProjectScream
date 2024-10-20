using UnityEngine;

namespace Game.Scripts.Core
{
    public interface IClickable
    {
        public bool CanBeClicked(GameObject actor);
        public void Click(GameObject actor);
    }
}
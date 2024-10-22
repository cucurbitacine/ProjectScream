using Game.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    [DisallowMultipleComponent]
    public class ButtonSprite : MonoBehaviour, IClickable, IHighlightable
    {
        [SerializeField] private bool available = true;
        
        [Space]
        [SerializeField] private UnityEvent clicked = new UnityEvent();
        
        public bool CanBeClicked(GameObject actor)
        {
            return available;
        }

        public void Click(GameObject actor)
        {
            clicked.Invoke();
        }

        public void Highlight(bool value)
        {
        }
    }
}

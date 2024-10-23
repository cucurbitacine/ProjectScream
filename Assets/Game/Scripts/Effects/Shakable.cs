using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Effects
{
    public class Shakable : MonoBehaviour, IHighlightable
    {
        public void Shake()
        {
            gameObject.Shake();
        }
        
        public void Highlight(bool value)
        {
            if (value)
            {
                Shake();
            }
        }
    }
}
using System;
using UnityEngine;

namespace Game.Scripts.Core
{
    public interface IDraggable
    {
        public event Action<bool> Dragged; 
        
        public Vector2 position { get; set; }
        
        public void OnDrag(GameObject context);
        public void OnDrop(GameObject context);
    }
}
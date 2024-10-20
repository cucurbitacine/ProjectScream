using System;
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts
{
    [DisallowMultipleComponent]
    public class Draggable : MonoBehaviour, IDraggable
    {
        public event Action<bool> Dragged;
        
        public Vector2 position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void OnDrag(GameObject context)
        {
            Dragged?.Invoke(true);
        }

        public void OnDrop(GameObject context)
        {
            Dragged?.Invoke(false);
        }
    }
}
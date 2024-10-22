using System.Collections.Generic;
using CucuTools;
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IDraggable))]
    public class Droppable : MonoBehaviour
    {
        [SerializeField] private  bool dropped;
        [SerializeField] private GameObject dropEffectPrefab;
        
        private IDraggable _draggable;

        private readonly List<RaycastHit2D> overlap = new List<RaycastHit2D>();

        private bool TryDrop(IBin bin)
        {
            if (!dropped && bin.CanDrop(gameObject))
            {
                bin.Drop(gameObject);

                dropped = true;
                
                return true;
            }

            return false;
        }
        
        private void Drop()
        {
            var filter = new ContactFilter2D()
            {
                useTriggers = true,
                useLayerMask = false,
            };

            var count = Physics2D.Raycast(_draggable.position, Vector2.zero, filter, overlap);

            if (count <= 0) return;
            
            for (var i = 0; i < count; i++)
            {
                var hit = overlap[i];

                if (hit.transform.TryGetComponent(out IBin bin) && TryDrop(bin))
                {
                    break;
                }
            }
        }
        
        private void OnDragged(bool dragged)
        {
            if (dragged) return;

            Drop();
        }
        
        private void Awake()
        {
            TryGetComponent(out _draggable);
        }

        private void OnEnable()
        {
            _draggable.Dragged += OnDragged;
            
            dropped = false;
        }
        
        private void OnDisable()
        {
            _draggable.Dragged -= OnDragged;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.TryGetComponent(out IBin bin) && TryDrop(bin))
            {
                if (dropEffectPrefab)
                {
                    var point = other.GetContact(0).point;

                    var effect = SmartPrefab.SmartInstantiate(dropEffectPrefab, point, Quaternion.identity);
                    effect.PlaySafe();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Game.Scripts.Core;
using Game.Scripts.Inputs;
using UnityEngine;

namespace Game.Scripts
{
    [DisallowMultipleComponent]
    public class DragSystem : MonoBehaviour, IDragSystem
    {
        [field: SerializeField] public bool IsDragging { get; private set; }

        [Space]
        [SerializeField] [Range(0f, 1f)] private float blendForceAtPosition = 0.1f;
        [SerializeField] private LayerMask dragLayer = 1;
        [SerializeField] private DragInput dragInput;

        private IDraggable _draggable;
        private Vector2 _offset;

        private readonly List<RaycastHit2D> overlap = new List<RaycastHit2D>();
        
        public void Drag(IDraggable draggable)
        {
            if (IsDragging) return;
            
            _draggable = draggable;
            _offset = _draggable.position - dragInput.WorldPoint;

            IsDragging = true;
            
            _draggable.OnDrag(gameObject);

            if (_draggable is Component component && component.TryGetComponent(out Rigidbody2D rigid2d))
            {
                rigid2d.velocity = Vector2.zero;
                rigid2d.angularVelocity = 0f;

                rigid2d.bodyType = RigidbodyType2D.Static;
            }
        }
        
        public void Drop()
        {
            if (!IsDragging) return;

            if (_draggable is Component component && component.TryGetComponent(out Rigidbody2D rigid2d))
            {
                rigid2d.bodyType = RigidbodyType2D.Dynamic;

                rigid2d.AddForce(velocity * (1f - blendForceAtPosition), ForceMode2D.Impulse);
                rigid2d.AddForceAtPosition(velocity * blendForceAtPosition, dragInput.WorldPoint, ForceMode2D.Impulse);
            }
            
            _draggable.OnDrop(gameObject);
            
            IsDragging = false;
            _draggable = null;
        }

        private void OnWorldPointEvent(Vector2 value)
        {
            if (IsDragging)
            {
                _draggable.position = value + _offset;
            }
        }

        private void OnClickEvent(bool clicked)
        {
            if (clicked && !IsDragging)
            {
                var filter = new ContactFilter2D()
                {
                    useTriggers = true,
                    useLayerMask = true,
                    layerMask = dragLayer,
                };

                var count = Physics2D.Raycast(dragInput.WorldPoint, Vector2.zero, filter, overlap);

                for (var i = 0; i < count; i++)
                {
                    var hit = overlap[i];

                    if (hit.transform.TryGetComponent(out IDraggable draggable))
                    {
                        Drag(draggable);
                        
                        break;
                    }
                }
            }
            else if (!clicked && IsDragging)
            {
                Drop();
            }
        }
        
        private void OnEnable()
        {
            IDragSystem.Master ??= this;

            dragInput.WorldPointEvent += OnWorldPointEvent;
            dragInput.ClickEvent += OnClickEvent;
        }

        private void OnDisable()
        {
            if (IDragSystem.Master is DragSystem drag && drag == this)
            {
                IDragSystem.Master = null;
            }

            dragInput.WorldPointEvent -= OnWorldPointEvent;
            dragInput.ClickEvent -= OnClickEvent;
        }

        private Vector2 velocity;
        private Vector2 lastPosition;
        
        private void FixedUpdate()
        {
            if (IsDragging && _draggable != null)
            {
                velocity = (_draggable.position - lastPosition) / Time.fixedDeltaTime;
                lastPosition = _draggable.position;
            }
        }
    }
}
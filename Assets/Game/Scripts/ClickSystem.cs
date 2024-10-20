using System.Collections.Generic;
using Game.Scripts.Core;
using Game.Scripts.Inputs;
using UnityEngine;

namespace Game.Scripts
{
    public class ClickSystem : MonoBehaviour
    {
        [SerializeField] private LayerMask clickLayer = 1;
        
        [Header("Input")]
        [SerializeField] private DragInput dragInput;

        private readonly List<RaycastHit2D> overlap = new List<RaycastHit2D>();

        private void OnPointEvent(Vector2 value)
        {
        }
        
        private void OnClickEvent(bool clicked)
        {
            if (!clicked) return;
            
            var filter = new ContactFilter2D()
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = clickLayer,
            };
                
            var count = Physics2D.Raycast(dragInput.WorldPoint, Vector2.zero, filter, overlap);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var hit = overlap[i];

                    if (hit.transform.TryGetComponent(out IClickable clickable) && clickable.CanBeClicked(gameObject))
                    {
                        clickable.Click(gameObject);
                            
                        break;
                    }
                }
            }
        }
        
        private void OnEnable()
        {
            dragInput.WorldPointEvent += OnPointEvent;
            dragInput.ClickEvent += OnClickEvent;
        }
        
        private void OnDisable()
        {
            dragInput.WorldPointEvent -= OnPointEvent;
            dragInput.ClickEvent -= OnClickEvent;
        }
    }
}

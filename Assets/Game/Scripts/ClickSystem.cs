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

        private int Raycast(Vector2 point)
        {
            var filter = new ContactFilter2D()
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = clickLayer,
            };
            
            return Physics2D.Raycast(point, Vector2.zero, filter, overlap);
        }

        private readonly HashSet<IHighlightable> highlightSet = new HashSet<IHighlightable>();
        private readonly HashSet<IHighlightable> highlightCurrent = new HashSet<IHighlightable>();
        
        private void OnWorldPointEvent(Vector2 value)
        {
            var count = Raycast(value);
            
            /*
             * Get Current Objects
             */
            
            highlightCurrent.Clear();
            for (var i = 0; i < count; i++)
            {
                if (overlap[i].transform.TryGetComponent(out IHighlightable highlightable))
                {
                    highlightCurrent.Add(highlightable);
                }
            }

            /*
             * Turn Off and Remove Lost Objects
             */
            
            foreach (var highlightable in highlightSet)
            {
                if (!highlightCurrent.Contains(highlightable))
                {
                    highlightable.Highlight(false);
                }
            }
            highlightSet.RemoveWhere(h => !highlightCurrent.Contains(h));

            /*
             * Add and Turn On New Objects
             */
            
            foreach (var highlightable in highlightCurrent)
            {
                if (highlightSet.Add(highlightable))
                {
                    highlightable.Highlight(true);
                }
            }
        }
        
        private void OnClickEvent(bool clicked)
        {
            if (!clicked) return;
            
            var count = Raycast(dragInput.WorldPoint);

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
            dragInput.WorldPointEvent += OnWorldPointEvent;
            dragInput.ClickEvent += OnClickEvent;
        }
        
        private void OnDisable()
        {
            dragInput.WorldPointEvent -= OnWorldPointEvent;
            dragInput.ClickEvent -= OnClickEvent;
        }
    }
}

using CucuTools;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using UnityEngine;

namespace Game.Scripts.UI
{
    [DisallowMultipleComponent]
    public class ClickableSlotDisplay : MonoBehaviour, IClickable, IBin, IHighlightable
    {
        [SerializeField] private bool canBeClicked = true;
        [SerializeField] private bool canBeDropped = true;
        
        [Space]
        [SerializeField] private SlotDisplay slotDisplay;

        [Space]
        [SerializeField] private AudioSfx clickSfx;
        [SerializeField] private AudioSfx highlightSfx;
        
        public bool CanBeClicked(GameObject actor)
        {
            return canBeClicked && slotDisplay.Slot != null && !slotDisplay.Slot.IsFree() && IDragSystem.Master != null && !IDragSystem.Master.IsDragging;
        }

        public void Click(GameObject actor)
        {
            if (CanBeClicked(actor) && slotDisplay.Slot.TryPeek(out var item) && item is IPrefabSource prefabSource)
            {
                if (clickSfx)
                {
                    gameObject.PlayOneShot(clickSfx.AudioClips);
                }
                
                var ingredient = SmartPrefab.SmartInstantiate(prefabSource.GetPrefab(), transform.position, Quaternion.identity);

                if (ingredient.TryGetComponent(out IDraggable draggable) && slotDisplay.Slot.TryPick(item))
                {
                    IDragSystem.Master.Drag(draggable);
                }
                else
                {
                    SmartPrefab.SmartDestroy(ingredient);
                }
            }
        }

        public bool CanDrop(GameObject target)
        {
            if (canBeDropped && target.TryGetComponent(out IItem item))
            {
                if (slotDisplay.Slot != null)
                {
                    return slotDisplay.Slot.Available(item.Config) > 0;
                }
            }

            return false;
        }

        public void Drop(GameObject target)
        {
            if (target.TryGetComponent(out IItem item) && slotDisplay.Slot.TryPut(item.Config))
            {
                SmartPrefab.SmartDestroy(target);
            }
        }
        
        public void Highlight(bool value)
        {
            if (value && canBeClicked && slotDisplay && slotDisplay.Slot != null && !slotDisplay.Slot.IsFree())
            {
                gameObject.Shake(0.5f);
                
                if (highlightSfx)
                {
                    gameObject.PlayOneShot(highlightSfx.AudioClips);
                }
            }
        }
    }
}

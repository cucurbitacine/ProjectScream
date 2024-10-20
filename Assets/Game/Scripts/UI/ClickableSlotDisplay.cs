using CucuTools;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.UI
{
    [DisallowMultipleComponent]
    public class ClickableSlotDisplay : MonoBehaviour, IClickable
    {
        [SerializeField] private SlotDisplay slotDisplay;
        
        public bool CanBeClicked(GameObject actor)
        {
            return slotDisplay.Slot != null && !slotDisplay.Slot.IsFree() && IDragSystem.Master != null && !IDragSystem.Master.IsDragging;
        }

        public void Click(GameObject actor)
        {
            if (CanBeClicked(actor) && slotDisplay.Slot.TryPeek(out var item) && item is ItemConfig config)
            {
                var ingredient = SmartPrefab.SmartInstantiate(config.GetPrefab(), transform.position, Quaternion.identity);

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
    }
}

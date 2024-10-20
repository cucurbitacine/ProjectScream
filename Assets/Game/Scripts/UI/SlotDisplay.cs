using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    [DisallowMultipleComponent]
    public class SlotDisplay : MonoBehaviour, ISlotDisplay
    {
        [SerializeField] private bool hideContainer = false;
        [SerializeField] private GameObject container;
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private TMP_Text amountDisplay;

        public ISlot Slot { get; private set; }
        
        public void Display(ISlot slot)
        {
            Slot = slot;

            if (Slot != null && Slot.TryPeek(out var item) && item is ItemConfig ingredient)
            {
                container?.SetActive(true);
                
                icon.sprite = ingredient.GetIcon();
                amountDisplay.text = $"{slot.CountItems}";

                gameObject.Shake();
            }
            else
            {
                container?.SetActive(!hideContainer);
                
                icon.sprite = null;
                amountDisplay.text = string.Empty;
            }
        }
    }
}
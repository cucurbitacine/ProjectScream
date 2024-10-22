using CucuTools.InventorySystem;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class DesireDisplay : MonoBehaviour
    {
        [SerializeField] private Customer customer;

        [SerializeField] private GameObject displayObject;

        private Slot _slot;
        private ISlotDisplay _slotDisplay;
        
        private void OnCustomerDesireChanged(Ingredient desire)
        {
            if (_slot == null) _slot = new Slot();

            _slot.Clear();
            
            if (desire.item && desire.amount > 0)
            {
                _slot.Put(desire.item, desire.amount);
            }
            
            _slotDisplay.Display(_slot);
        }

        private void Awake()
        {
            displayObject.TryGetComponent(out _slotDisplay);
        }

        private void OnEnable()
        {
            customer.DesireChanged += OnCustomerDesireChanged;
            
            OnCustomerDesireChanged(customer.Desire);
        }

        private void OnDisable()
        {
            customer.DesireChanged -= OnCustomerDesireChanged;
        }
    }
}
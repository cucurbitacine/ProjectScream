using System;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using UnityEngine;

namespace Game.Scripts
{
    public class Customer : MonoBehaviour, IHighlightable
    {
        [SerializeField] private Ingredient desire;
        [SerializeField] [Min(0)] private int uselessItemPrice = 0;
        [SerializeField] private ItemDropArea dropArea;
        
        [Space]
        [SerializeField] private GameObject desireDisplay;
        [SerializeField] private DesireSource desireSource;
        
        [Space]
        [SerializeField] private GameObject inventory;
        
        private IInventory _inventory;
        
        public event Action<Ingredient> DesireChanged;
        
        public Ingredient Desire => desire;

        public event Action<Customer> Completed;
        
        public void Ready()
        {
            desireDisplay?.SetActive(false);

            dropArea.Available = false;
        }

        public void Activate()
        {
            dropArea.Available = true;
            
            desireDisplay?.SetActive(true);
            
            UpdateDesire();
        }
        
        public void Sale()
        {
            gameObject.Shake();

            var usefulAmount = _inventory.Pick(desire.item, desire.amount);
            var usefulPricePerItem = (desire.item is ICostSource cost ? cost.GetCost() : uselessItemPrice);
            var usefulPriceTotal = usefulPricePerItem * usefulAmount;

            var uselessAmount = _inventory.CountItems();
            var uselessPrice = uselessItemPrice * uselessAmount;
            
            _inventory.Clear();
            
            if (desire.amount == usefulAmount)
            {
                Reputation.Instance.Add(1);
            }
            else if (usefulAmount == 0)
            {
                Reputation.Instance.Get(1);
            }
            
            Wallet.Instance.Add(usefulPriceTotal + uselessPrice);

            Complete();
        }

        public void Complete()
        {
            dropArea.Available = false;
            
            desireDisplay?.SetActive(false);
            
            Completed?.Invoke(this);
        }
        
        public void Highlight(bool value)
        {
            if (value)
            {
                gameObject.Shake(0.5f);
            }
        }

        public void SetDesireSource(DesireSource newDesireSource)
        {
            desireSource = newDesireSource;
        }
        
        public void UpdateDesire()
        {
            desire = desireSource.CreateDesire();

            DesireChanged?.Invoke(desire);
        }
        
        private void OnInventoryUpdated(IInventory arg1, ISlot arg2)
        {
            if (_inventory.CountItems(desire.item) == desire.amount)
            {
                Sale();
            }
        }
        
        private void Awake()
        {
            if (inventory == null) inventory = gameObject;

            inventory.TryGetComponent(out _inventory);
        }

        private void OnEnable()
        {
            _inventory.InventoryUpdated += OnInventoryUpdated;
        }
        
        private void OnDisable()
        {
            _inventory.InventoryUpdated -= OnInventoryUpdated;
        }
    }
}
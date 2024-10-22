using System;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using UnityEngine;

namespace Game.Scripts
{
    public class Buyer : MonoBehaviour, IHighlightable
    {
        [SerializeField] private Ingredient desire;
        [SerializeField] [Min(0)] private int uselessItemPrice = 0;

        [Space]
        [SerializeField] private DesireSource desireSource;
        
        [Space]
        [SerializeField] private GameObject desireDisplay;
        [SerializeField] private GameObject inventory;
        
        private IInventory _inventory;
        
        public event Action<Ingredient> DesireChanged;
        
        public Ingredient Desire => desire;
        
        public void Sale()
        {
            gameObject.Shake();
            
            var price = (desire.item is ICostSource cost ? cost.GetCost() : uselessItemPrice) * _inventory.Pick(desire.item, desire.amount);

            var uselessPrice = uselessItemPrice * _inventory.CountItems();
            
            _inventory.Clear();
            
            Wallet.Instance.Add(price + uselessPrice);
            
            UpdateDesire();
        }

        public void UpdateDesire()
        {
            desire = desireSource.CreateDesire();

            DesireChanged?.Invoke(desire);
        }
        
        private void Awake()
        {
            if (inventory == null) inventory = gameObject;

            inventory.TryGetComponent(out _inventory);
        }

        private void Start()
        {
            UpdateDesire();
        }

        public void Highlight(bool value)
        {
            if (desireDisplay)
            {
                desireDisplay.SetActive(value);
            }
            
            if (value)
            {
                gameObject.Shake(0.5f);
            }
        }
    }
}
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts
{
    public class SaleArea : MonoBehaviour, IClickable
    {
        [SerializeField] private Ingredient desire;

        
        [Space]
        [SerializeField] private GameObject inventory;
        
        private IInventory _inventory;
        
        public bool CanBeClicked(GameObject actor)
        {
            return _inventory.CountItems() > 0;
        }

        public void Click(GameObject actor)
        {
            
        }

        private void Awake()
        {
            if (inventory == null) inventory = gameObject;

            inventory.TryGetComponent(out _inventory);
        }
    }
}
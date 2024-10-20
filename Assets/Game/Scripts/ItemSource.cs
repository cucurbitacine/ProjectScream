using System;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using UnityEngine;

namespace Game.Scripts
{
    public class ItemSource : MonoBehaviour, IClickable
    {
        [SerializeField] private bool available = true;
        [SerializeField] private ItemBase item;
        
        [Space]
        [SerializeField] private int progress = 0;
        [SerializeField] private int totalClick = 10;

        [Space]
        [SerializeField] private GameObject destination;
        
        private IInventory _inventory;
        
        public bool CanBeClicked(GameObject actor)
        {
            if (!available) return false;

            if (gameObject.IsShaking()) return false;
            
            return true;
        }

        public void Click(GameObject actor)
        {
            progress++;

            if (totalClick <= progress && _inventory.TryPut(item))
            {
                progress = 0;
            }
            
            gameObject.Shake();
        }
        
        private void Awake()
        {
            destination.TryGetComponent(out _inventory);
        }
    }
}

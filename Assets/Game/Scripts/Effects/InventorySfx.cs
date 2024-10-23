using System;
using CucuTools;
using CucuTools.InventorySystem;
using UnityEngine;

namespace Game.Scripts.Effects
{
    [RequireComponent(typeof(IInventory))]
    [RequireComponent(typeof(AudioSource))]
    public class InventorySfx : MonoBehaviour
    {
        [SerializeField] private AudioSfx updateSfx;
        
        private IInventory _inventory;
        
        private void InventoryUpdated(IInventory arg1, ISlot arg2)
        {
            if (updateSfx)
            {
                gameObject.PlayOneShot(updateSfx.AudioClips);
            }
        }
        
        private void Awake()
        {
            TryGetComponent(out _inventory);
        }

        private void OnEnable()
        {
            _inventory.InventoryUpdated += InventoryUpdated;
        }

        private void OnDisable()
        {
            _inventory.InventoryUpdated -= InventoryUpdated;
        }
    }
}
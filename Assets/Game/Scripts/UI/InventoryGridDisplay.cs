using System.Collections.Generic;
using CucuTools;
using CucuTools.InventorySystem;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class InventoryGridDisplay : MonoBehaviour
    {
        [SerializeField] [Min(1)] private Vector2Int sizeInventory = Vector2Int.one;    
        [SerializeField] private Vector2 sizeSlot = Vector2.one;
        [SerializeField] [Min(0f)] private Vector2 space = Vector2.zero;
        [SerializeField] private Vector2 offset = Vector2.zero;    
        
        [Space]
        [SerializeField] private GameObject inventorySource;
        
        [Space]
        [SerializeField] private Transform slotContainer;
        [SerializeField] private GameObject slotDisplayPrefab;
        
        private IInventory _inventory;

        private readonly Dictionary<ISlot, ISlotDisplay> dict = new Dictionary<ISlot, ISlotDisplay>();

        private Vector2 GetPositionByNumber(int number)
        {
            var index = Vector2Int.zero;
            
            index.y = number / sizeInventory.x;
            index.x = number - index.y * sizeInventory.x;

            index.y = sizeInventory.y - index.y - 1;
            
            var point = Vector2.Scale(sizeSlot, index) + Vector2.Scale(space, index);
            
            return (Vector2)transform.position + offset + point + sizeSlot * 0.5f;
        }
        
        private void OnInventoryUpdated(IInventory inv, ISlot slt)
        {
            if (!dict.TryGetValue(slt, out var display))
            {
                var index = dict.Count;
                var position = GetPositionByNumber(index);

                var displayObject = SmartPrefab.SmartInstantiate(slotDisplayPrefab, position, Quaternion.identity, slotContainer);

                if (displayObject.TryGetComponent(out display))
                {
                    dict.Add(slt, display);
                }
                else
                {
                    SmartPrefab.SmartDestroy(displayObject);
                    return;
                }
            }
            
            display.Display(slt);
        }
        
        private void Awake()
        {
            if (inventorySource == null) inventorySource = gameObject;

            inventorySource.TryGetComponent(out _inventory);

            if (slotContainer == null) slotContainer = transform;
        }

        private void OnEnable()
        {
            _inventory.InventoryUpdated += OnInventoryUpdated;
        }
        
        private void OnDisable()
        {
            _inventory.InventoryUpdated -= OnInventoryUpdated;
        }

        private void Start()
        {
            for (var i = 0; i < _inventory.CountSlots; i++)
            {
                OnInventoryUpdated(_inventory, _inventory.GetSlot(i));
            }
        }

        private void OnDrawGizmos()
        {
            var amount = sizeInventory.x * sizeInventory.y;

            for (var i = 0; i < amount; i++)
            {
                Gizmos.DrawWireCube(GetPositionByNumber(i), sizeSlot);
            }
        }
    }
}
using System.Collections.Generic;
using CucuTools;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using UnityEngine;

namespace Game.Scripts
{
    public class Pot : MonoBehaviour, IClickable
    {
        [SerializeField] private GameObject destinationObject;

        [Space]
        [SerializeField] private ItemConfig failResult;
        [SerializeField] private BookRecipe _bookRecipe;
        
        private IInventory _inventory;

        public Ingredient GetResult()
        {
            var stack = new List<Ingredient>();

            foreach (var item in _inventory.GetItems())
            {
                if (item is ItemConfig itemConfig)
                {
                    var amount = _inventory.CountItems(itemConfig);
                    
                    var itemStack = new Ingredient()
                    {
                        amount = amount,
                        item = itemConfig,
                    };
                        
                    stack.Add(itemStack);
                }
            }

            if (_bookRecipe && _bookRecipe.TryGetRecipe(out var bestRecipe, stack))
            {
                return bestRecipe.GetResult();
            }

            if (failResult)
            {
                return new Ingredient()
                {
                    item = failResult,
                    amount = _inventory.CountItems(),
                };
            }
            
            return default;
        }
        
        public void Craft()
        {
            var result = GetResult();

            if (result.amount == 0)
            {
                return;
            }

            if (destinationObject && destinationObject.TryGetComponent(out IInventory destination))
            {
                if (destination == _inventory)
                {
                    _inventory.Clear();
                    
                    _inventory.Put(result.item, result.amount);

                    return;
                }

                if (destination.TryPut(result.item, result.amount))
                {
                    _inventory.Clear();

                    return;
                }
            }

            if (result.item is ItemConfig itemConfig)
            {
                for (var i = 0; i < result.amount; i++)
                {
                    SmartPrefab.SmartInstantiate(itemConfig.GetPrefab(), transform.position, Quaternion.identity);
                }
            }
        }
        
        private void OnInventoryUpdated(IInventory inv, ISlot slt)
        {
            gameObject.Shake();
        }

        private void Awake()
        {
            TryGetComponent(out _inventory);
        }

        private void OnEnable()
        {
            _inventory.InventoryUpdated += OnInventoryUpdated;
        }

        private void OnDisable()
        {
            _inventory.InventoryUpdated -= OnInventoryUpdated;
        }

        public bool CanBeClicked(GameObject actor)
        {
            return true;
        }
        
        public void Click(GameObject actor)
        {
            Craft();

            gameObject.Shake();
        }
    }
}
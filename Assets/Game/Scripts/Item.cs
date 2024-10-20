using CucuTools.InventorySystem;
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts
{
    [DisallowMultipleComponent]
    public class Item : MonoBehaviour, IItem
    {
        [field: SerializeField] public ItemBase Config { get; private set; }
        
        public void SetConfig(ItemBase config)
        {
            Config = config;
        }
    }
}
using CucuTools.InventorySystem;
using UnityEngine;

namespace Game.Scripts.Core
{
    [CreateAssetMenu(menuName = "Game/Create Item Config", fileName = "Item Config", order = 0)]
    public class ItemConfig : ItemBase
    {
        [Space]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite icon;

        public GameObject GetPrefab()
        {
            if (prefab.TryGetComponent<IItem>(out var item))
            {
                item.SetConfig(this);
            }
            
            return prefab;
        }

        public Sprite GetIcon()
        {
            return icon;
        }
    }
}
using CucuTools;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts
{
    public class ItemDropArea : MonoBehaviour, IBin
    {
        [SerializeField] private GameObject destination;

        private IInventory _destination;
        
        public bool CanDrop(GameObject target)
        {
            return target.TryGetComponent<IItem>(out _);
        }

        public void Drop(GameObject target)
        {
            if (target.TryGetComponent(out IItem item) && _destination.TryPut(item.Config))
            {
                SmartPrefab.SmartDestroy(target);
            }
        }

        private void Awake()
        {
            if (destination == null) destination = gameObject;

            destination.TryGetComponent(out _destination);
        }
    }
}
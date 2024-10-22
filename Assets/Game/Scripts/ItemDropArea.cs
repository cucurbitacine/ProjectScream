using System.Linq;
using CucuTools;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts
{
    public class ItemDropArea : MonoBehaviour, IBin
    {
        [field: SerializeField] public bool Available { get; set; } = true;
        
        [Space]
        [SerializeField] private GameObject destination;

        private IInventory _destination;
        
        public bool CanDrop(GameObject target)
        {
            if (!Available) return false;

            if (GetComponentsInChildren<IFilter>().Any(filter => !filter.Filter(target)))
            {
                return false;
            }
            
            return target.TryGetComponent<IItem>(out var item) && _destination.Available(item.Config) > 0;
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

    public interface IFilter
    {
        public bool Filter(GameObject target);
    }
}
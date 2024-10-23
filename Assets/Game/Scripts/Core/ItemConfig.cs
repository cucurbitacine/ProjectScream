using CucuTools.InventorySystem;
using UnityEngine;

namespace Game.Scripts.Core
{
    [CreateAssetMenu(menuName = "Game/Create Item Config", fileName = "Item Config", order = 0)]
    public class ItemConfig : ItemBase, IPrefabSource, IIconSource, ICostSource, IDurationSource, ITitleSource
    {
        [Header("Config")]
        [SerializeField] private string title = string.Empty;
        [Space]
        [SerializeField] [Min(0f)] private float duration = 0f;
        [SerializeField] [Min(0)] private int cost = 0;
        
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

        public int GetCost()
        {
            return cost;
        }

        public float GetDuration()
        {
            return duration;
        }

        public string GetTitle()
        {
            return title;
        }
    }

    public interface IPrefabSource
    {
        public GameObject GetPrefab();
    }
    
    public interface IIconSource
    {
        public Sprite GetIcon();
    }
    
    public interface ICostSource
    {
        public int GetCost();
    }

    public interface IDurationSource
    {
        public float GetDuration();
    }

    public interface ITitleSource
    {
        public string GetTitle();
    }
}
using CucuTools.InventorySystem;

namespace Game.Scripts.Core
{
    public interface IItem
    {
        public ItemBase Config { get; }
        public void SetConfig(ItemBase config);
    }
}
namespace Game.Scripts.Core
{
    public interface IDragSystem
    {
        public static IDragSystem Master { get; protected set; }
        
        public bool IsDragging { get; }

        public void Drag(IDraggable draggable);
        public void Drop();
    }
}
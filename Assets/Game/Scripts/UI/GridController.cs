using UnityEngine;

namespace Game.Scripts.UI
{
    [DisallowMultipleComponent]
    public class GridController : MonoBehaviour
    {
        [field: SerializeField, Min(1)] public Vector2Int GridSize { get; set; } = Vector2Int.one;    
        [field: SerializeField] public Vector2 CellSize { get; set; } = Vector2.one;
        [field: SerializeField, Min(0f)] public Vector2 Space { get; set; } = Vector2.zero;
        [field: SerializeField] public Vector2 Offset { get; set; } = Vector2.zero;
        
        public int Length => GridSize.x * GridSize.y;

        public Vector2 GetPosition(Vector2Int index)
        {
            index.x %= GridSize.x;
            index.y %= GridSize.y;
            
            index.y = GridSize.y - index.y - 1;
            
            var point = Vector2.Scale(CellSize, index) + Vector2.Scale(Space, index);
            
            return (Vector2)transform.position + Offset + point + CellSize * 0.5f;
        }

        public Vector2 GetPosition(int x, int y)
        {
            return GetPosition(new Vector2Int(x, y));
        }
        
        public Vector2 GetPositionByNumber(int number)
        {
            var index = Vector2Int.zero;
            
            index.y = number / GridSize.x;
            index.x = number - index.y * GridSize.x;

            return GetPosition(index);
        }
        
        private void OnDrawGizmos()
        {
            var amount = GridSize.x * GridSize.y;

            for (var i = 0; i < amount; i++)
            {
                Gizmos.DrawWireCube(GetPositionByNumber(i), CellSize);
            }
        }
    }
}
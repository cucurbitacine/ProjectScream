using UnityEngine;

namespace Game.Scripts
{
    public class DesireSource : MonoBehaviour
    {
        [SerializeField] private Ingredient defaultDesire;
        
        public Ingredient CreateDesire()
        {
            return defaultDesire;
        }
    }
}

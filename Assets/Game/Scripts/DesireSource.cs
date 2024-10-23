using UnityEngine;

namespace Game.Scripts
{
    public class DesireSource : MonoBehaviour
    {
        [SerializeField] private DesireList desireList;
        
        public Ingredient CreateDesire()
        {
            return desireList.CreateDesire();
        }
    }
}
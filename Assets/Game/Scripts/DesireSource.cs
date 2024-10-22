using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class DesireSource : MonoBehaviour
    {
        [SerializeField] private Ingredient defaultDesire;
        [SerializeField] private List<Ingredient> list = new List<Ingredient>();
        
        public Ingredient CreateDesire()
        {
            if (list != null && list.Count > 0)
            {
                var index = Random.Range(0, list.Count);

                var desire = list[index];

                if (desire.item && desire.amount > 0)
                {
                    return desire;
                }
            }
            
            return defaultDesire;
        }
    }
}

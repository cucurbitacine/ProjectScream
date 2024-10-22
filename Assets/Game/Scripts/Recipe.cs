using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.InventorySystem;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Game/Create Recipe", fileName = "Recipe", order = 0)]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private Ingredient result;
        [Space]
        [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();

        public Ingredient GetResult()
        {
            return result;
        }

        public List<Ingredient> GetIngredients()
        {
            return new List<Ingredient>(ingredients);
        }
        
        public bool Match(List<Ingredient> listStack)
        {
            foreach (var ingredient in ingredients)
            {
                var item = ingredient.item;
                var amount = listStack.Where(i => i.item == item).Sum(i => i.amount);

                if (ingredient.amount != amount) return false;
            }
            
            foreach (var stack in listStack)
            {
                var item = stack.item;
                var amount = ingredients.Where(i => i.item == item).Sum(i => i.amount);

                if (stack.amount != amount) return false;
            }
            
            return true;
        }
    }

    [Serializable]
    public struct Ingredient
    {
        public int amount;
        public ItemBase item;
    }
}

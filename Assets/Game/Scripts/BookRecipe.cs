using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Game/Create Book Recipe", fileName = "Book Recipe", order = 0)]
    public class BookRecipe : ScriptableObject
    {
        [SerializeField] private List<Recipe> recipes = new List<Recipe>();

        public bool TryGetRecipe(out Recipe result, List<Ingredient> ingredients)
        {
            foreach (var recipe in recipes.Where(recipe => recipe.Match(ingredients)))
            {
                result = recipe;
                return true;
            }

            result = null;
            return false;
        }
    }
}
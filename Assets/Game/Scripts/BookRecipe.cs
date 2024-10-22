using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Game/Create Book Recipe", fileName = "Book Recipe", order = 0)]
    public class BookRecipe : ScriptableObject
    {
        [SerializeField] private List<Recipe> recipes = new List<Recipe>();

        public int CountRecipes => recipes.Count;

        public Recipe GetRecipe(int index)
        {
            if (0 <= index && index < CountRecipes)
            {
                return recipes[index];
            }

            return null;
        }
        
        public bool FindRecipe(out Recipe found, List<Ingredient> ingredients)
        {
            foreach (var recipe in recipes.Where(recipe => recipe.Match(ingredients)))
            {
                found = recipe;
                return true;
            }

            found = null;
            return false;
        }
    }
}
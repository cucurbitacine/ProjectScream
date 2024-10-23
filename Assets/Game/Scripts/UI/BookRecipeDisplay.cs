using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class BookRecipeDisplay : MonoBehaviour
    {
        [SerializeField] [Min(0)] private int selected = 0;
        [SerializeField] private BookRecipe currentBook;

        [Space]
        [SerializeField] private TMP_Text pageDisplay;
        [SerializeField] private RecipeDisplay recipeDisplay;
        
        public void Display()
        {
            recipeDisplay.Display(currentBook.GetRecipe(selected));

            pageDisplay.text = $"{(selected + 1)} / {currentBook.CountRecipes}";
        }
        
        public void Display(BookRecipe bookRecipe)
        {
            currentBook = bookRecipe;
            
            selected = 0;

            Display();
        }

        public void NextPage()
        {
            selected = (selected + 1) % currentBook.CountRecipes;
            
            Display();
        }

        public void PreviousPage()
        {
            if (selected == 0)
            {
                selected = currentBook.CountRecipes - 1;
            }
            else
            {
                selected--;
            }
            
            Display();
        }

        private void Start()
        {
            if (currentBook) Display(currentBook);
        }
    }
}

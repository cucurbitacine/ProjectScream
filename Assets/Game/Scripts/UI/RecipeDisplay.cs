using System.Collections.Generic;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class RecipeDisplay : MonoBehaviour
    {
        [SerializeField] private Recipe currentRecipe;
        
        [Space]
        [SerializeField] private SlotDisplay resultSlotDisplay;
        [SerializeField] private List<SlotDisplay> ingredientSlotDisplays = new List<SlotDisplay>();

        [Space]
        [SerializeField] private TMP_Text titleDisplay;
        
        private ISlot _resultSlot;
        
        public void Display()
        {
            var result = currentRecipe.GetResult();
            _resultSlot ??= new Slot();
            _resultSlot.Clear();
            _resultSlot.Put(result.item, result.amount);
            resultSlotDisplay.Display(_resultSlot);

            if (titleDisplay)
            {
                var title = result.item is ITitleSource source ? source.GetTitle() : string.Empty;

                titleDisplay.text = title;
            }
            
            var ingredients = currentRecipe.GetIngredients();
            for (var i = 0; i < ingredientSlotDisplays.Count; i++)
            {
                var slotDisplay = ingredientSlotDisplays[i];
                
                if (i < ingredients.Count)
                {
                    slotDisplay.gameObject.SetActive(true);

                    var ingredientSlot = new Slot();
                    ingredientSlot.Put(ingredients[i].item, ingredients[i].amount);
                    
                    slotDisplay.Display(ingredientSlot);
                }
                else
                {
                    slotDisplay.gameObject.SetActive(false);
                }
            }
        }
        
        public void Display(Recipe recipe)
        {
            currentRecipe = recipe;
            
            Display();
        }
    }
}
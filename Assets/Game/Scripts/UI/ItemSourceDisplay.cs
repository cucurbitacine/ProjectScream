using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class ItemSourceDisplay : MonoBehaviour
    {
        [SerializeField] private ItemSource itemSource;
        
        [Space]
        [SerializeField] private TMP_Text textDisplay;

        private void OnUnlocked(bool unlocked)
        {
            if (unlocked)
            {
                OnAvailableChanged(itemSource.AvailableAmount);
            }
            else
            {
                textDisplay.text = $"{itemSource.UnlockCost}$";
            }
        }
        
        private void OnAvailableChanged(int available)
        {
            textDisplay.text = $"{available} / {itemSource.TotalAmount}";
        }
        
        private void OnCooldownChanged(float progress)
        {
            var timeLeft = itemSource.CooldownTime * (1f - progress);
            textDisplay.text = $"{timeLeft:F1} s";
        }
        
        private void OnEnable()
        {
            itemSource.Unlocked += OnUnlocked;
            itemSource.AvailableChanged += OnAvailableChanged;
            itemSource.CooldownChanged += OnCooldownChanged;
        }
        
        private void OnDisable()
        {
            itemSource.Unlocked -= OnUnlocked;
            itemSource.AvailableChanged -= OnAvailableChanged;
            itemSource.CooldownChanged -= OnCooldownChanged;
        }

        private void Start()
        {
            OnUnlocked(itemSource.IsUnlocked);
        }
    }
}
using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class WalletDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text amountDisplay;

        private static Wallet Wallet => Wallet.Instance;
        
        private void OnWalletAmountChanged(int amount)
        {
            amountDisplay.text = $"{amount}$";
        }
        
        private void OnEnable()
        {
            Wallet.AmountChanged += OnWalletAmountChanged;
        }
        
        private void OnDisable()
        {
            Wallet.AmountChanged -= OnWalletAmountChanged;
        }

        private void Start()
        {
            OnWalletAmountChanged(Wallet.Amount);
        }
    }
}

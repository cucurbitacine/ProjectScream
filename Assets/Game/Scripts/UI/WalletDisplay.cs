using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class WalletDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text amountDisplay;

        private static Wallet Wallet => Wallet.Instance;

        private Tweener _shaking;
        
        private void OnWalletAmountChanged(int amount)
        {
            amountDisplay.text = $"{amount}$";

            if (_shaking != null && _shaking.IsActive() && _shaking.IsPlaying())
            {
                _shaking.Complete();
            }

            _shaking = transform.DOShakeScale(0.2f, 0.1f);
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

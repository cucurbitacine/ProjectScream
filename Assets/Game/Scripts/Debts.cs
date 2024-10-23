using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class Debts : MonoBehaviour
    {
        [SerializeField] private int debts = 4500;

        [Space] [SerializeField] private TMP_Text display;

        public void UpdateDebts()
        {
            display.text = $"Pay Debts: {debts}$";
        }

        public void Pay()
        {
            var amount = Mathf.Min(Wallet.Instance.Amount, debts);
            if (Wallet.Instance.Get(amount))
            {
                debts -= amount;

                debts = Mathf.Max(0, debts);
            }
            
            UpdateDebts();
        }
        
        private void Start()
        {
            UpdateDebts();
        }
    }
}
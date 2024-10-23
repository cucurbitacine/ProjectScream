using System;
using System.Collections;
using CucuTools.InventorySystem;
using Game.Scripts.Core;
using Game.Scripts.Effects;
using UnityEngine;

namespace Game.Scripts
{
    public class ItemSource : MonoBehaviour, IClickable, IHighlightable
    {
        [SerializeField] private ItemBase item;
        
        [Space]
        [SerializeField] private bool isUnlocked = false;
        [SerializeField] [Min(0)] private int unlockCost = 0;

        [Space]
        [SerializeField] private int pickedAmount = 0;
        [SerializeField] private int totalAmount = 10;

        [Space]
        [SerializeField] private bool isCooldown = false;
        [SerializeField] [Min(0f)] private float cooldownTime = 1f;
        
        [Space]
        [SerializeField] private GameObject destination;
        
        private IInventory _destination;

        public bool IsUnlocked => isUnlocked;
        public int UnlockCost => unlockCost;
        public float CooldownTime => cooldownTime;
        public int TotalAmount => totalAmount;
        public int AvailableAmount => TotalAmount - pickedAmount;
        
        public event Action<bool> Unlocked; 
        public event Action<int> AvailableChanged; 
        public event Action<float> CooldownChanged; 
        
        public bool CanBeClicked(GameObject actor)
        {
            return true;
        }

        public void Click(GameObject actor)
        {
            //if (gameObject.IsShaking()) return;
            
            gameObject.Shake();
            
            if (!isUnlocked)
            {
                if (Wallet.Instance.Get(unlockCost))
                {
                    isUnlocked = true;
                    Unlocked?.Invoke(true);
                }
                
                return;
            }

            if (!isCooldown && _destination.TryPut(item))
            {
                pickedAmount++;
                AvailableChanged?.Invoke(AvailableAmount);
                
                if (totalAmount <= pickedAmount)
                {
                    Cooldown();
                }
            }
        }

        public void Highlight(bool value)
        {
            if (value)
            {
                gameObject.Shake(0.5f);
            }
        }
        
        private void Cooldown()
        {
            if (isCooldown) return;
            
            if (cooldownTime > 0f)
            {
                StartCoroutine(CooldownProgress());
            }
            else
            {
                pickedAmount = 0;
            }
        }

        private IEnumerator CooldownProgress()
        {
            isCooldown = true;

            var timer = 0f;
            while (timer < cooldownTime)
            {
                CooldownChanged?.Invoke(timer / cooldownTime);
                
                timer += Time.deltaTime;
                yield return null;
            }
            
            CooldownChanged?.Invoke(1f);
            isCooldown = false;
            
            pickedAmount = 0;
            AvailableChanged?.Invoke(AvailableAmount);
        }
        
        private void Awake()
        {
            destination.TryGetComponent(out _destination);
        }

        private void Start()
        {
            Unlocked?.Invoke(isUnlocked);
        }
    }
}

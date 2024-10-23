using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class ReputationDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text amountDisplay;

        private static Reputation Reputation => Reputation.Instance;

        private Tweener _shaking;
        
        private void OnReputationLevelChanged(int amount)
        {
            amountDisplay.text = $"{amount}";
            
            if (_shaking != null && _shaking.IsActive() && _shaking.IsPlaying())
            {
                _shaking.Complete();
            }

            _shaking = transform.DOShakeScale(0.2f, 0.1f);
        }
        
        private void OnEnable()
        {
            Reputation.LevelChanged += OnReputationLevelChanged;
        }
        
        private void OnDisable()
        {
            Reputation.LevelChanged -= OnReputationLevelChanged;
        }

        private void Start()
        {
            OnReputationLevelChanged(Reputation.Level);
        }
    }
}
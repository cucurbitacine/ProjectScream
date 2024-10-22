using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class ReputationDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text amountDisplay;

        private static Reputation Reputation => Reputation.Instance;
        
        private void OnReputationLevelChanged(int amount)
        {
            amountDisplay.text = $"{amount}";
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
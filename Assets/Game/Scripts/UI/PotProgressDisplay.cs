using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class PotProgressDisplay : MonoBehaviour
    {
        [SerializeField] private Pot pot;

        [Space] [SerializeField] private TMP_Text progressDisplay;
        
        private void OnPotProgressChanged(float progress)
        {
            var onEdge = Mathf.Approximately(progress, 0f) || Mathf.Approximately(progress, 1f);
            progressDisplay.gameObject.SetActive(!onEdge);

            progressDisplay.text = $"{(int)(100 * progress):F0}%";
        }
        
        private void OnEnable()
        {
            pot.ProgressChanged += OnPotProgressChanged;
        }
        
        private void OnDisable()
        {
            pot.ProgressChanged -= OnPotProgressChanged;
        }

        private void Start()
        {
            OnPotProgressChanged(0f);
        }
    }
}
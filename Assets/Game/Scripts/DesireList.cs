using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Game/Create Desire List", fileName = "Desire List", order = 0)]
    public class DesireList : ScriptableObject
    {
        [SerializeField] private Ingredient defaultDesire;
        [SerializeField] private List<DesireByLevel> desireByLevel = new List<DesireByLevel>();
        
        public Ingredient CreateDesire()
        {
            var level = GetLevel();

            var randomNumber = Random.Range(0, level + 1);

            var list = desireByLevel[randomNumber];
            
            Debug.Log($"Reputation: {Reputation.Instance.Level}. Level: {level}. List with reputation [{list.reputation}] was taken");
            
            return list.GetRandom();

            return defaultDesire;
        }

        private int GetLevel()
        {
            var level = -1;
            
            for (var i = desireByLevel.Count - 1; i >= 0; i--)
            {
                if (desireByLevel[i].reputation <= Reputation.Instance.Level)
                {
                    level++;
                }
            }
            
            return level;
        }

        private void OnLevelChanged(int reputation)
        {
            Debug.Log($"Reputation: {reputation}. Level: {GetLevel()}");
        }
        
        private void OnEnable()
        {
            Reputation.Instance.LevelChanged += OnLevelChanged;

            OnLevelChanged(Reputation.Instance.Level);
        }

        private void OnDisable()
        {
            Reputation.Instance.LevelChanged -= OnLevelChanged;
        }
    }

    [Serializable]
    public struct DesireByLevel
    {
        public int reputation;
        [SerializeField] private List<Ingredient> desires;

        public Ingredient GetRandom()
        {
            return desires[Random.Range(0, desires.Count)];
        }
    }
}
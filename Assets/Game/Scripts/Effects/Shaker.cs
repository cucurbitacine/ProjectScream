using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Effects
{
    [DisallowMultipleComponent]
    public class Shaker : MonoBehaviour
    {
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float strength = 0.5f;

        [Space]
        [SerializeField] private Transform target;

        private float _lastShake = float.MinValue;

        public bool IsShaking => Time.time - _lastShake < duration;
        
        public void Shake()
        {
            if (target == null) target = transform;

            if (IsShaking) return;
            
            _lastShake = Time.time;
            
            target.DOShakeScale(duration, strength, 10, 90f, true, ShakeRandomnessMode.Harmonic);
        }
    }

    public static class ShakeExt
    {
        public static bool IsShaking(this GameObject gameObject)
        {
            return gameObject.TryGetComponent(out Shaker shaker) && shaker.IsShaking;
        }
        
        public static void Shake(this GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out Shaker shaker))
            {
                shaker.Shake();
            }
        }
    }
}

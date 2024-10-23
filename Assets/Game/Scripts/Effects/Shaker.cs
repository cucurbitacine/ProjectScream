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

        public bool IsShaking => _shaking != null && _shaking.IsActive() && _shaking.IsPlaying();

        private Tweener _shaking;
        
        public void Shake(float power = 1f)
        {
            if (target == null) target = transform;

            if (IsShaking)
            {
                _shaking.Complete();
            }
            
            _shaking = target.DOShakeScale(duration * power, strength * power, 10, 90f, true, ShakeRandomnessMode.Harmonic);
        }
    }

    public static class ShakeExt
    {
        public static bool IsShaking(this GameObject gameObject)
        {
            return gameObject.TryGetComponent(out Shaker shaker) && shaker.IsShaking;
        }
        
        public static void Shake(this GameObject gameObject, float power = 1f)
        {
            if (gameObject.TryGetComponent(out Shaker shaker))
            {
                shaker.Shake(power);
            }
        }
    }
}

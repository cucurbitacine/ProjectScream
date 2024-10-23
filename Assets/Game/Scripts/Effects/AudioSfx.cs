using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Effects
{
    [CreateAssetMenu(menuName = "Game/Create AudioSfx", fileName = "Audio Sfx", order = 0)]
    public class AudioSfx : ScriptableObject
    {
        [field: SerializeField] public List<AudioClip> AudioClips { get; private set; } = new List<AudioClip>();
    }

    public static class SfxExt
    {
        public static void PlaySafe(this GameObject gameObject, List<AudioClip> clips)
        {
            if (gameObject.TryGetComponent(out AudioSource source))
            {
                if (source.isPlaying) source.Stop();

                source.clip = clips[Random.Range(0, clips.Count)];

                source.Play();
            }
        }
    }
}
using CucuTools;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class ApplicationVersionDisplay : MonoBehaviour
    {
        private LazyComponent<TMP_Text> _lazyText;

        public TMP_Text Text => (_lazyText ??= new LazyComponent<TMP_Text>(gameObject)).Value;

        public string GetVersion()
        {
            return $"{Application.version} {Application.platform} {Application.unityVersion}";
        }

        public void UpdateVersion()
        {
            Text.text = $"{GetVersion()}";
        }
        
        private void OnEnable()
        {
            UpdateVersion();
        }
    }
}

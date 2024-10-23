using System;
using UnityEngine;

namespace Game.Scripts
{
    public class Dev : MonoBehaviour
    {
        [SerializeField] private bool dev;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKeyDown(KeyCode.V))
            {
                dev = !dev;
            }
            
            if (dev)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Reputation.Instance.Add(1);
                }
                
                if (Input.GetKeyDown(KeyCode.T))
                {
                    Wallet.Instance.Add(100);
                }
            }
        }

        private void OnGUI()
        {
            if (dev)
            {
                GUILayout.Box("DEV MODE");
            }
        }
    }
}
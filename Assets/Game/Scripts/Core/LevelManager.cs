using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Core
{
    public class LevelManager : MonoBehaviour
    {
        public void GoMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void GoTutorial()
        {
            SceneManager.LoadScene(1);
        }

        public void GoPlay()
        {
            SceneManager.LoadScene(2);
        }
    }
}
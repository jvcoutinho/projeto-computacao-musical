using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class SceneSwitcher : MonoBehaviour
    {
        public string NextScene;

        public void SwitchScene()
        {
            SceneManager.LoadScene(NextScene);
        }
    }
}
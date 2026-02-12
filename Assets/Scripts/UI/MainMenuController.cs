using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImbaLife.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string gameplayScene = "HouseScene";

        public void StartGame()
        {
            SceneManager.LoadScene(gameplayScene);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Loads the scene named "Play"
    public void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // Quits the application
    public void Quit()
    {
        // Works in built game
        Application.Quit();

        // Helpful for testing in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using UnityEditor;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        GlobalManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

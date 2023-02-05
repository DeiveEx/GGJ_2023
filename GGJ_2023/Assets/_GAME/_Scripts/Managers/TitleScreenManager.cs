using UnityEditor;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _optionsPanel;

    private void Start()
    {
        ShowMainPanel();
    }

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

    public void ShowMainPanel()
    {
        _optionsPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }
    
    public void ShowOptionsPanel()
    {
        _optionsPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }
}

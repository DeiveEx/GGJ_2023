using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : SimpleSingleton<GlobalManager>
{
    [SerializeField] private string _titleScreen;
    [SerializeField] private string _gameplayScreen;
    [SerializeField] private string _gameOverScreen;
    [SerializeField] private bool _allowGameOver;

    private GameData _gameData = new();

    public GameData GameData => _gameData;
    public bool AllowGameOver => _allowGameOver;

    public void ShowTitleScreen()
    {
        SceneManager.LoadScene(_titleScreen);
    }
    
    public void ShowGameplayScreen()
    {
        SceneManager.LoadScene(_gameplayScreen);
    }
    
    public void ShowGameOverScreen()
    {
        SceneManager.LoadScene(_gameOverScreen);
    }

    public void StartGame()
    {
        _gameData = new GameData();
        ShowGameplayScreen();
    }
}

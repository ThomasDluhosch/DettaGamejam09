using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
    }

    private GameState currentState = GameState.Playing;

    public GameState CurrentState => currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        Debug.Log("Game Paused.");
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        Debug.Log("Game Resumed.");
    }

    public void EndGame()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        Debug.Log("Game Over.");
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        Debug.Log("Game Restarted.");
        currentState = GameState.Playing;
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        Debug.Log("Returned to Main Menu.");
        currentState = GameState.Playing;
    }
}
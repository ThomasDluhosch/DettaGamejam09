using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
        }

        if (optionsButton != null)
        {
            optionsButton.onClick.AddListener(OpenOptions);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
        MusicManager.Instance.PlayMusic(gameMusic);
        Debug.LogWarning("Keine Szene ausgewählt!");
    }

    void OpenOptions()
    {
        Debug.LogWarning("Keine Optionen ausgewählt!");
    }

    void QuitGame()
    {
        Debug.Log("Spiel beendet!");
        Application.Quit();
    }
}

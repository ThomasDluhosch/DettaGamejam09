using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

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

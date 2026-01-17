using TMPro;
using UnityEngine;

public class GameoverMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerText;

    private readonly string[] playerNames = { "Crane Operator", "Climber" };
    public void ShowMenu(int winnerIndex)
    {
        gameObject.SetActive(true);

        winnerText.text = $"{playerNames[winnerIndex]} Wins!";

        GameManager.Instance.EndGame();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }
}

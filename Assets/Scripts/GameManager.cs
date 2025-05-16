using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject titleScreen;    // Title screen canvas
    public GameObject inGameUI;       // In-game UI canvas
    public GameObject gameOverUI;     // Game over canvas
    public Text timerText;            // Legacy UI Text for countdown

    [Header("Timer Settings")]
    public int startTime = 99;

    private int currentTime;
    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f;

        if (inGameUI != null)
            inGameUI.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (timerText != null)
            timerText.text = startTime.ToString("D2");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed. Quitting game...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;

        if (titleScreen != null)
            titleScreen.SetActive(false);

        if (inGameUI != null)
            inGameUI.SetActive(true);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        currentTime = startTime;
        StartCoroutine(Countdown());

        Time.timeScale = 1f;
        Debug.Log("Game Started");
    }

    private IEnumerator Countdown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;

            if (timerText != null)
                timerText.text = currentTime.ToString("D2");
        }

        GameOver();
    }

    private void GameOver()
    {
        Debug.Log("Time's up! Game Over.");

        if (inGameUI != null)
            inGameUI.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
}

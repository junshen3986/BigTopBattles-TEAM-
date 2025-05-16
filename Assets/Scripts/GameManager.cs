using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject titleScreen;     // Title screen canvas
    public GameObject inGameUI;        // In-game UI canvas
    public GameObject gameOverUI;      // Game over canvas
    public Text timerText;          // Legacy UI Text for countdown
    public Text gameOverText;       // Text component inside gameOverUI

    [Header("Health Bars")]
    public Slider player1HealthSlider;
    public Slider player2HealthSlider;

    [Header("Timer Settings")]
    public int startTime = 99;

    int currentTime;
    bool gameStarted = false;
    bool gameOver = false;

    void Start()
    {
        Time.timeScale = 0f;

        if (inGameUI != null) inGameUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (titleScreen != null) titleScreen.SetActive(true);

        if (timerText != null)
            timerText.text = startTime.ToString("D2");
    }

    void Update()
    {
        // Quit on ESC
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();

        // In Game: check for health‐based victory
        if (gameStarted && !gameOver)
        {
            if (player1HealthSlider != null && player1HealthSlider.value <= 0f)
                EndGame("Player 2 Wins!");
            else if (player2HealthSlider != null && player2HealthSlider.value <= 0f)
                EndGame("Player 1 Wins!");
        }

        // After Game Over: restart with R
        if (gameOver && Input.GetKeyDown(KeyCode.R))
            Restart();
    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;

        if (titleScreen != null) titleScreen.SetActive(false);
        if (inGameUI != null) inGameUI.SetActive(true);
        if (gameOverUI != null) gameOverUI.SetActive(false);

        currentTime = startTime;
        StartCoroutine(Countdown());

        Time.timeScale = 1f;
    }

    IEnumerator Countdown()
    {
        while (currentTime > 0 && !gameOver)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;

            if (timerText != null)
                timerText.text = currentTime.ToString("D2");
        }

        if (!gameOver)
            EndGame("Time's Up!");
    }

    void EndGame(string message)
    {
        gameOver = true;

        // Stop the timer coroutine
        StopAllCoroutines();

        if (inGameUI != null) inGameUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = message;

        Time.timeScale = 0f;
    }

    void Restart()
    {
        // reload the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

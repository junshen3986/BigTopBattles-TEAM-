using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public Text timerText;       // Assign your UI Text element in Inspector
    public int startTime = 99;   // Start counting down from 99

    private int currentTime;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            UpdateTimerText();
        }

        // Timer has reached zero
        GameOver();
    }

    private void UpdateTimerText()
    {
        timerText.text = currentTime.ToString("D2"); // Formats 1 as "01"
    }

    private void GameOver()
    {
        Debug.Log("Time's up! Game over.");
        Time.timeScale = 0f; // Stops all gameplay
    }
}

using UnityEngine;
using TMPro;

public class TotalTime : MonoBehaviour
{
    public TMP_Text totalTimer;
    public bool isRunning = false;

    private float totalSeconds = 0f;
    private float lastSavedTime;

    private const string SaveKey = "TotalPlayTime";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        totalSeconds = PlayerPrefs.GetFloat(SaveKey, 0f);
    }

    void Update()
    {
        if (isRunning)
        {
            totalSeconds += Time.unscaledDeltaTime; // unaffected by Time.timeScale
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        if (totalTimer != null)
        {
            int hours = Mathf.FloorToInt(totalSeconds / 3600f);
            int minutes = Mathf.FloorToInt((totalSeconds % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(totalSeconds % 60f);
            totalTimer.text = $"{hours:00}:{minutes:00}:{seconds:00}";
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
        SaveTime();
    }

    public void ResetTImer()
    {
        totalSeconds = 0f;
        SaveTime();
        UpdateTimerText();
    }

    public void SaveTime()
    {
        PlayerPrefs.SetFloat(SaveKey, totalSeconds);
        PlayerPrefs.Save();
    }
}

using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public int currentScore = 0;

    public TMP_Text roundScoreText;
    public int roundScore = 0;

    public TMP_Text shopPoints;
    public int shopScore = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
        roundScore = 0;

        UpdateScoreText();
    }

    public void AddPoints (int comboScore)
    {
        int scoreToAdd = comboScore * 2;

        currentScore += scoreToAdd;
        roundScore += scoreToAdd;
        shopScore += scoreToAdd;

        UpdateScoreText();


        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.Save();

        Debug.Log($"Added {scoreToAdd} points! Round: {roundScore}, Total: {currentScore}");
    }

    public void ResetRoundScore()
    {
        roundScore = 0;
        UpdateScoreText();
    }

    public void RemovePoints(int points) 
    { 
        currentScore -= points;
        shopScore -= points;
        UpdateScoreText();

        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();

        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.Save();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();

        if (scoreText != null)
            shopPoints.text = currentScore.ToString();

        if (roundScoreText != null)
            roundScoreText.text = roundScore.ToString();
    }
}

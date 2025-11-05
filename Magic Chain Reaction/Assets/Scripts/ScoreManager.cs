using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class ScoreManager : MonoBehaviour, IUpgradeListener
{
    public TMP_Text scoreText;
    public int currentScore = 0;

    public TMP_Text roundScoreText;
    public int roundScore = 0;

    public TMP_Text shopPoints;
    public int shopScore = 0;

    public float scoreMultiplier = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.ApplyUpgradesToScene();

        currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
        roundScore = 0;

        UpdateScoreText();
    }

    public void OnUpgradesApplied(UpgradeManager upgrades)
    {
        scoreMultiplier = upgrades.scoreMultiplier;
    }



    public void AddPoints (int basePoints)
    {
        int total = Mathf.RoundToInt(basePoints * scoreMultiplier);

        currentScore += total;
        roundScore += total;
        shopScore += total;

        UpdateScoreText();


        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.Save();

        Debug.Log($"Added {scoreMultiplier} points! Round: {roundScore}, Total: {currentScore}");
    }

    public void ResetRoundScore()
    {
        roundScore = 0;
        UpdateScoreText();
    }

    public void RemovePoints(int points) 
    {
        currentScore -= points;
        if (currentScore < 0) currentScore = 0;

        shopScore = currentScore; // keep shopPoints in sync

        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.Save();

        UpdateScoreText();
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

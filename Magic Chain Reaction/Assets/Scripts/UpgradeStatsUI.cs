using TMPro;
using UnityEngine;

public class UpgradeStatsUI : MonoBehaviour, IUpgradeListener
{
    public TMP_Text moreCirclesText;
    public TMP_Text extraShotsText;
    public TMP_Text fasterProjectilesText;
    public TMP_Text increasePointsText;
    public TMP_Text moreTimeText;
    public TMP_Text projectileCountText;
    public TMP_Text fasterCircleSpawnText;

    

    private bool isVisible = true;

    void Start()
    {
        UpdateUI();
        gameObject.SetActive(isVisible);
    }

  

    public void UpdateUI()
    {
        var u = UpgradeManager.Instance;
        if (u == null) return;

        moreCirclesText.text = $"More Circles: {u.moreCirclesLevel} (+{u.moreCirclesLevel} circles)";
        extraShotsText.text = $"Extra Shots: {u.extraShotsLevel} (+{u.extraShotsLevel} max shots)";
        fasterProjectilesText.text = $"Faster Projectiles: {u.fasterProjectilesLevel} (+{u.projectileSpeed} speed)";
        increasePointsText.text = $"Increase Points: {u.increasePointsLevel} (x{u.scoreMultiplier})";
        moreTimeText.text = $"More Time: {u.moreTimeLevel} (+{u.moreTimeLevel * 5}s)";
        projectileCountText.text = $"Projectile Count: {u.projectileCountLevel} (+{u.targetProjectileCount} shots per target)";
        fasterCircleSpawnText.text = $"Faster Circle Spawn: {u.fasterCircleSpawnLevel} (-{u.fasterCircleSpawnLevel * 0.2f}s interval)";
    }

    public void OnUpgradesApplied(UpgradeManager upgrades)
    {
        UpdateUI();
    }
}

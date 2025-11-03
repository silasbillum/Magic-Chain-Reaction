using TMPro;
using UnityEngine;

public class UpgradeStatsUI : MonoBehaviour
{
    public TMP_Text moreCirclesText;
    public TMP_Text fasterCirclesText;
    public TMP_Text moreProjectilesText;
    public TMP_Text fasterProjectilesText;
    public TMP_Text morePointsText;
    public TMP_Text moreTimeText;
    public TMP_Text multiPlayText;

    void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        var u = UpgradeManager.Instance;
        if (u == null) return;

        moreCirclesText.text = $"More Circles: {u.moreCirclesLevel} (+{u.moreCirclesLevel} circles)";
        fasterCirclesText.text = $"Faster Circles: {u.fasterProjectilesLevel} (-{u.fasterProjectilesLevel * 0.2f}s interval)";
        moreProjectilesText.text = $"More Projectiles: {u.moreProjectilesLevel} (+{u.moreProjectilesLevel} shots)";
        fasterProjectilesText.text = $"Faster Projectiles: {u.fasterProjectilesLevel} (+{u.fasterProjectilesLevel * 2f} speed)";
        morePointsText.text = $"Increase Points: {u.morePointsLevel} (x{1f + u.morePointsLevel * 0.25f})";
        moreTimeText.text = $"More Time: {u.moreTimeLevel} (+{u.moreTimeLevel * 5} sec)";
        multiPlayText.text = $"Projectiles Count: {u.multiPlayLevel} (Level {u.multiPlayLevel})";
    }
}

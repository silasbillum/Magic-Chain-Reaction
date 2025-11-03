using UnityEngine;

[DefaultExecutionOrder(-100)]
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("=== Upgrade Levels ===")]
    [Range(0, 50)] public int moreCirclesLevel = 0;           // circles per spawn
    [Range(0, 50)] public int extraShotsLevel = 0;            // max shots per round
    [Range(0, 50)] public int fasterProjectilesLevel = 0;     // player projectile speed
    [Range(0, 50)] public int increasePointsLevel = 0;        // score multiplier
    [Range(0, 50)] public int moreTimeLevel = 0;              // round timer bonus
    [Range(0, 50)] public int projectileCountLevel = 0;       // projectiles spawned by target/enemy
    [Range(0, 50)] public int fasterCircleSpawnLevel = 0;     // interval reduction for circle spawn

    [Header("=== Derived Values ===")]
    public float circleInterval => Mathf.Max(0.1f, 1f - fasterCircleSpawnLevel * 0.2f);
    public int circleSpawnCount => 3 + moreCirclesLevel;
    public int maxShots => 1 + extraShotsLevel;
    public float projectileSpeed => 10f + fasterProjectilesLevel * 2f;
    public float scoreMultiplier => 1f + increasePointsLevel * 0.25f;
    public float roundTime => 30f + moreTimeLevel * 5f;
    public int targetProjectileCount => 1 + projectileCountLevel * 2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveUpgrades()
    {
        PlayerPrefs.SetInt("MoreCircles", moreCirclesLevel);
        PlayerPrefs.SetInt("ExtraShots", extraShotsLevel);
        PlayerPrefs.SetInt("FasterProjectiles", fasterProjectilesLevel);
        PlayerPrefs.SetInt("IncreasePoints", increasePointsLevel);
        PlayerPrefs.SetInt("MoreTime", moreTimeLevel);
        PlayerPrefs.SetInt("ProjectileCount", projectileCountLevel);
        PlayerPrefs.SetInt("FasterCircleSpawn", fasterCircleSpawnLevel);
        PlayerPrefs.Save();
    }

    public void LoadUpgrades()
    {
        moreCirclesLevel = PlayerPrefs.GetInt("MoreCircles", 0);
        extraShotsLevel = PlayerPrefs.GetInt("ExtraShots", 0);
        fasterProjectilesLevel = PlayerPrefs.GetInt("FasterProjectiles", 0);
        increasePointsLevel = PlayerPrefs.GetInt("IncreasePoints", 0);
        moreTimeLevel = PlayerPrefs.GetInt("MoreTime", 0);
        projectileCountLevel = PlayerPrefs.GetInt("ProjectileCount", 0);
        fasterCircleSpawnLevel = PlayerPrefs.GetInt("FasterCircleSpawn", 0);
    }

    public void ApplyUpgradesToScene()
    {
        var listeners = FindObjectsOfType<MonoBehaviour>(true);
        foreach (var listener in listeners)
        {
            if (listener is IUpgradeListener upgradeListener)
                upgradeListener.OnUpgradesApplied(this);
        }
    }

    public void ResetUpgrades()
    {
        PlayerPrefs.DeleteAll();
        LoadUpgrades();
        ApplyUpgradesToScene();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // When changed in Inspector, instantly apply to all scene objects
        if (!Application.isPlaying) return;
        ApplyUpgradesToScene();
    }
#endif
}

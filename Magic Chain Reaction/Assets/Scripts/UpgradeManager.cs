using UnityEngine;

[DefaultExecutionOrder(-100)] // ensures it initializes early
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("=== Upgrade Levels (Editable in Inspector) ===")]
    [Range(0, 50)] public int moreCirclesLevel = 0;
    [Range(0, 50)] public int moreProjectilesLevel = 0;
    [Range(0, 50)] public int fasterProjectilesLevel = 0;
    [Range(0, 50)] public int morePointsLevel = 0;
    [Range(0, 50)] public int moreTimeLevel = 0;
    [Range(0, 50)] public int multiPlayLevel = 0;

    [Header("=== Derived Debug Info (Read-Only) ===")]
    [SerializeField] private float circleSpawnInterval;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float scoreMultiplier;
    [SerializeField] private float roundTime;

    [ContextMenu("Save Upgrades")]
    private void SaveUpgradesMenu() => SaveUpgrades();

    [ContextMenu("Reset Upgrades")]
    private void ResetUpgradesMenu() => ResetUpgrades();

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Recalculate derived values when sliders are changed
        CalculateDebugValues();

        // Apply upgrades live in the editor if the game is running
        if (Application.isPlaying && Instance != null)
        {
            Instance.ApplyUpgradesToScene();
        }
    }
#endif



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUpgrades();
            CalculateDebugValues();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveUpgrades()
    {
        PlayerPrefs.SetInt("MoreCircles", moreCirclesLevel);
        PlayerPrefs.SetInt("MoreProjectiles", moreProjectilesLevel);
        PlayerPrefs.SetInt("FasterProjectiles", fasterProjectilesLevel);
        PlayerPrefs.SetInt("MorePoints", morePointsLevel);
        PlayerPrefs.SetInt("MoreTime", moreTimeLevel);
        PlayerPrefs.SetInt("MultiPlay", multiPlayLevel);
        PlayerPrefs.Save();

        CalculateDebugValues();
    }

    public void LoadUpgrades()
    {
        moreCirclesLevel = PlayerPrefs.GetInt("MoreCircles", 0);
        moreProjectilesLevel = PlayerPrefs.GetInt("MoreProjectiles", 0);
        fasterProjectilesLevel = PlayerPrefs.GetInt("FasterProjectiles", 0);
        morePointsLevel = PlayerPrefs.GetInt("MorePoints", 0);
        moreTimeLevel = PlayerPrefs.GetInt("MoreTime", 0);
        multiPlayLevel = PlayerPrefs.GetInt("MultiPlay", 0);

        CalculateDebugValues();
    }

    public void ResetUpgrades()
    {
        PlayerPrefs.DeleteKey("MoreCircles");
        PlayerPrefs.DeleteKey("MoreProjectiles");
        PlayerPrefs.DeleteKey("FasterProjectiles");
        PlayerPrefs.DeleteKey("MorePoints");
        PlayerPrefs.DeleteKey("MoreTime");
        PlayerPrefs.DeleteKey("MultiPlay");
        LoadUpgrades();
    }

    private void CalculateDebugValues()
    {
        // Derived info for quick balancing in Inspector
        circleSpawnInterval = Mathf.Max(0.1f, 1f - (moreCirclesLevel * 0.2f));
        projectileSpeed = 10f + (fasterProjectilesLevel * 2f);
        scoreMultiplier = 1f + (morePointsLevel * 0.25f);
        roundTime = 30f + (moreTimeLevel * 5f);
    }

    public void ApplyUpgradesToScene()
    {
        var listeners = FindObjectsOfType<MonoBehaviour>(true);
        foreach (var listener in listeners)
        {
            if (listener is IUpgradeListener upgradeListener)
                upgradeListener.OnUpgradesApplied(this);
        }

        Debug.Log("[UpgradeManager] Applied upgrades to all listeners.");
    }
}

using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public int moreCirclesLevel = 0;
    public int moreProjectilesLevel = 0;
    public int fasterProjectilesLevel = 0;
    public int morePointsLevel = 0;
    public int moreTimeLevel = 0;
    public int multiPlayLevel = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep between scenes
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
        PlayerPrefs.SetInt("MoreProjectiles", moreProjectilesLevel);
        PlayerPrefs.SetInt("FasterProjectiles", fasterProjectilesLevel);
        PlayerPrefs.SetInt("MorePoints", morePointsLevel);
        PlayerPrefs.SetInt("MoreTime", moreTimeLevel);
        PlayerPrefs.SetInt("MultiPlay", multiPlayLevel);
        PlayerPrefs.Save();
    }

    public void LoadUpgrades()
    {
        moreCirclesLevel = PlayerPrefs.GetInt("MoreCircles", 0);
        moreProjectilesLevel = PlayerPrefs.GetInt("MoreProjectiles", 0);
        fasterProjectilesLevel = PlayerPrefs.GetInt("FasterProjectiles", 0);
        morePointsLevel = PlayerPrefs.GetInt("MorePoints", 0);
        moreTimeLevel = PlayerPrefs.GetInt("MoreTime", 0);
        multiPlayLevel = PlayerPrefs.GetInt("MultiPlay", 0);
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
}

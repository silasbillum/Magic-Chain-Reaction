using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject GameUI;
    public GameObject Menu;
    public GameObject LostMenu;
    public GameObject UpgradeMenu;


    public PointAndShoot pointAndShoot;
    public ComboSystem comboSystem;
    public ScoreManager scoreManager;
    public TotalTime totalTime;
    public Target target;
    public ShopManager shopManager;

    public static bool isRestarting = false;

    public void Start()
    {

        if (LostMenu == null) Debug.LogError("LostMenu reference missing!");
        if (Menu == null) Debug.LogError("Menu reference missing!");
        if (GameUI == null) Debug.LogError("GameUI reference missing!");
        if (UpgradeMenu == null) Debug.LogError("UpgradeMenu reference missing!");

        if (isRestarting)
        {           
            isRestarting = false;
            StartGame();
        }
        else
        {           
            ShowMainMenu();
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Only open LostMenu if the game is currently running
            if (GameUI.activeSelf)
            {
                GameOver();
            }
        }
    }


    public void ShowMainMenu()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;

        Menu.SetActive(true);
        GameUI.SetActive(false);
        LostMenu.SetActive(false);
        UpgradeMenu.SetActive(false);

        if (pointAndShoot != null)
            pointAndShoot.enabled = false;

        if (totalTime != null)
            totalTime.PauseTimer();
    }

    public void StartGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;

        GameUI.SetActive(true);
        Menu.SetActive(false);
        LostMenu.SetActive(false);
        UpgradeMenu.SetActive(false);

        if (pointAndShoot != null)
            pointAndShoot.enabled = true;

        if (comboSystem != null)
        {
            comboSystem.comboScore = 0;
            comboSystem.UpdateComboText();
        }

        if (scoreManager != null)
            scoreManager.ResetRoundScore();

        

        RoundTimer timer = FindFirstObjectByType<RoundTimer>();
        if (timer != null)
        {
            timer.ResetTimer();     // restore Inspector time
            timer.StartCountdown(); // start fresh
        }

        if (totalTime != null)
            totalTime.StartTimer();

       


        ClearAllObjectsWithTag("Fireball");
        ClearAllObjectsWithTag("Circle");

        UpgradeManager.Instance.ApplyUpgradesToScene();



        Debug.Log($"LostMenu active after StartGame: {LostMenu.activeSelf}");



    }

    public void GameOver()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;

        LostMenu.SetActive(true);
        GameUI.SetActive(false);
        Menu.SetActive(false);
        UpgradeMenu.SetActive(false);

        if (pointAndShoot != null)
            pointAndShoot.enabled = false;

        if (comboSystem != null && scoreManager != null)
            scoreManager.AddPoints(comboSystem.comboScore);
    }

    public void UpgradeShop()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;


        UpgradeMenu.SetActive(true);
        LostMenu.SetActive(false);
        GameUI.SetActive(false);
        Menu.SetActive(false);

        if (totalTime != null)
            totalTime.PauseTimer();
    }

    public void Restart()
    {
        isRestarting = true; 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NewGame()
    {
        Debug.Log("=== Starting New Game ===");

        // --- Reactivate gameplay UI ---
        Cursor.visible = false;

        isRestarting = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // --- Reset Shop ---
        if (shopManager != null)
            shopManager.ResetAllItems();

        GameUI.SetActive(true);
        Menu.SetActive(false);
        LostMenu.SetActive(false);
        UpgradeMenu.SetActive(false);

        // --- Re-enable player input ---
        if (pointAndShoot != null)
            pointAndShoot.enabled = true;

        // --- Reset score and combo ---
        if (scoreManager != null)
            scoreManager.ResetScore();

        if (comboSystem != null)
        {
            comboSystem.comboScore = 0;
            comboSystem.UpdateComboText();
        }

        // --- Clear active projectiles and circles ---
        ClearAllObjectsWithTag("Fireball");
        ClearAllObjectsWithTag("Circle");

        // --- Reset timer ---
        RoundTimer timer = FindFirstObjectByType<RoundTimer>();
        if (timer != null)
        {
            timer.ResetTimer();
            timer.StartCountdown();
        }

        if (totalTime != null)
            totalTime.ResetTImer();
            totalTime.StartTimer();

        // --- Reset upgrades ---
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ResetUpgrades();
            UpgradeManager.Instance.ApplyUpgradesToScene();

            // Update Upgrade UI immediately
            var statsUI = FindFirstObjectByType<UpgradeStatsUI>();
            if (statsUI != null)
                statsUI.UpdateUI();
        }

        


        // Reset upgrades
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ResetUpgrades();
            UpgradeManager.Instance.ApplyUpgradesToScene();
        }
        // --- Reset spawner ---
        var spawner = FindFirstObjectByType<SpawnInCameraView>();
        if (spawner != null)
        {
            spawner.enabled = false;
            spawner.enabled = true;
        }

        Debug.Log("New game started — everything reset to default.");
    }





    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!");
    }

    private void ClearAllObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    



}

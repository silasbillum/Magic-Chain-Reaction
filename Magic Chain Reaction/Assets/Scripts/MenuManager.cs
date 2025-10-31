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


        ClearAllObjectsWithTag("Fireball");
        ClearAllObjectsWithTag("Circle");


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
    }

    public void Restart()
    {
        isRestarting = true; 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NewGame()
    {
        // --- Reactivate gameplay UI first ---
        Cursor.visible = false;
        Time.timeScale = 1f;

        GameUI.SetActive(true);
        Menu.SetActive(false);
        LostMenu.SetActive(false);
        UpgradeMenu.SetActive(false);

        // --- Re-enable player input ---
        if (pointAndShoot != null)
            pointAndShoot.enabled = true;

        // --- Reset runtime systems ---
        if (scoreManager != null)
            scoreManager.ResetScore();

        if (comboSystem != null)
        {
            comboSystem.comboScore = 0;
            comboSystem.UpdateComboText();
        }

        // --- Clear active enemies or projectiles ---
        ClearAllObjectsWithTag("Fireball");
        ClearAllObjectsWithTag("Circle");

        // --- Reset and start timer ---
        RoundTimer timer = FindFirstObjectByType<RoundTimer>();
        if (timer != null)
        {
            timer.ResetTimer();     // restore Inspector-set value
            timer.StartCountdown(); // start fresh countdown
            Debug.Log($"Timer reset to {timer.CountdownTimer}");
        }
        else
        {
            Debug.LogWarning("RoundTimer not found in scene!");
        }

        // --- Reset persistent progress (optional) ---
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("New game started — all progress and runtime state reset.");
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

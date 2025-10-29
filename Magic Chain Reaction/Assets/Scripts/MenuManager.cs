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

    // Update is called once per frame
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

        if (FindFirstObjectByType<RoundTimer>() != null)
            FindFirstObjectByType<RoundTimer>().StartCountdown();

        if (scoreManager != null)
            scoreManager.ResetRoundScore();

        Debug.Log($"LostMenu active after StartGame: {LostMenu.activeSelf}");



    }

    public void GameOver()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;

        LostMenu.SetActive(true);
        GameUI.SetActive(true);
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

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!");
    }
}

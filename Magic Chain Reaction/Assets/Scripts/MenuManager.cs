using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject GameUI;
    public GameObject Menu;
    public GameObject LostMenu;


    public PointAndShoot pointAndShoot;

    public static bool isRestarting = false;

    public void Start()
    {
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

        if (pointAndShoot != null)
            pointAndShoot.enabled = true;
    }

    public void GameOver()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;

        LostMenu.SetActive(true);
        GameUI.SetActive(true);
        Menu.SetActive(false);

        if (pointAndShoot != null)
            pointAndShoot.enabled = false;
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

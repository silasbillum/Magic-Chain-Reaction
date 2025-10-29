using UnityEngine;
using TMPro;
using System.Collections;

public class RoundTimer : MonoBehaviour
{
    public TMP_Text roundTimer;
    public float CountdownTimer = 15f;
    public MenuManager menuManager;

    public void StartCountdown()
    {
        StopAllCoroutines();
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        while (CountdownTimer > 0)
        {
            roundTimer.text = CountdownTimer.ToString("0");
            yield return new WaitForSeconds(1f); // works fine now since Time.timeScale = 1
            CountdownTimer--;
        }

        roundTimer.text = "0";
        menuManager.GameOver();
    }
}

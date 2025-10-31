using UnityEngine;
using TMPro;
using System.Collections;
using static UnityEditor.ShaderGraph.Internal.Texture2DShaderProperty;

public class RoundTimer : MonoBehaviour
{
    public TMP_Text roundTimer;
    public float CountdownTimer;
    private float defaultTime;
    public MenuManager menuManager;

    private void Awake()
    {
        // Save the Inspector-assigned time so we can reset to it later
        defaultTime = CountdownTimer;
    }

    public void StartCountdown()
    {
        StopAllCoroutines();
        StartCoroutine(CountdownCoroutine());
    }

    public void ResetTimer()
    {
        CountdownTimer = defaultTime;
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

using UnityEngine;
using TMPro;
using System.Collections;

public class RoundTimer : MonoBehaviour
{
    public TMP_Text roundTimer;
    public float CountdownTimer = 15f;
   
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator StartCountdown()
    {
        
        while (CountdownTimer > 0)
        {
            roundTimer.text = CountdownTimer.ToString("0");
            yield return new WaitForSeconds(1.0f);
            CountdownTimer--;
        }

        roundTimer.text = "0";
        GameOver();
    }

    void GameOver()
    {
        Debug.Log("Game Over!!");
        
    }
}

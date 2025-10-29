using UnityEngine;
using TMPro;

public class ComboSystem : MonoBehaviour
{
    public TMP_Text comboText;
    public int comboScore = 0;
    public ScoreManager scoreManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void AddCombo()
    {
        comboScore++;
        UpdateComboText();

    }

   
    public void UpdateComboText()
    {
        if (comboText != null)
            comboText.text = comboScore.ToString();
    }
}

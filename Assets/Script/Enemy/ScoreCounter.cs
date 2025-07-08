using TMPro; // TextMeshPro���g���ꍇ
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI killText;
    private int killCount = 0;

    public void AddKill()
    {
        killCount++;
        UpdateText();
    }

    void UpdateText()
    {
        killText.text = "Score : " + killCount;
    }

    public int GetKillCount()
    {
        return killCount;
    }

}

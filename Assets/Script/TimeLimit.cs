using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeLimit : MonoBehaviour
{
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;
    public GameObject resultPanel; // ← リザルトUI
    public TextMeshProUGUI scoreText;
    public ScoreCounter killCounter; // ← スコア取得元

    private float timeLeft;

    void Start()
    {
        timeLeft = timeLimit;
        resultPanel.SetActive(false); // 非表示にしておく
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);

        if (timeLeft <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        timerText.text = "Time: 0";
        resultPanel.SetActive(true);

        // スコア反映
        int score = killCounter.GetKillCount();
        scoreText.text = "Score: " + score;
    }

    // ボタン用：再スタート
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ボタン用：タイトルへ戻る
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}

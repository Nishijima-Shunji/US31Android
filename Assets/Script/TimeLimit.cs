using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeLimit : MonoBehaviour
{
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;
    public GameObject resultPanel; // �� ���U���gUI
    public TextMeshProUGUI scoreText;
    public ScoreCounter killCounter; // �� �X�R�A�擾��

    private float timeLeft;

    void Start()
    {
        timeLeft = timeLimit;
        resultPanel.SetActive(false); // ��\���ɂ��Ă���
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

        // �X�R�A���f
        int score = killCounter.GetKillCount();
        scoreText.text = "Score: " + score;
    }

    // �{�^���p�F�ăX�^�[�g
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // �{�^���p�F�^�C�g���֖߂�
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PointManagerTst : MonoBehaviour
{
    public static PointManagerTst instance;

    public int score = 0;
    public Text scoreText;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }
}

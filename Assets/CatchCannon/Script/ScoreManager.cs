// using UnityEngine;
// using TMPro;

// public class ScoreManager : MonoBehaviour
// {

//     public static ScoreManager Instance;
//     public int score = 0;
//     public TextMeshProUGUI scoreText;
//     private int nextCoinRainThreshold = 100;


//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     public void AddScore(int amount)
//     {
//         score += amount;
//         scoreText.text = "Score: " + score;
//         // if (score >= nextCoinRainThreshold)
//         // {
//         //     CoinRainEvent coinRainEvent = FindAnyObjectByType<CoinRainEvent>();
//         //     if (coinRainEvent !=null) 
//         //     {
//         //         coinRainEvent.TriggerCoinRain();
//         //     }
//         // }
//     }
// }
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    private int nextCoinRainThreshold = 100;

    public event Action OnCoinRain; // Event for coin rain

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
        if (score >= nextCoinRainThreshold)
        {
            OnCoinRain?.Invoke(); // Raise event
            nextCoinRainThreshold += 100; // Increase threshold for next rain
        }
    }
}
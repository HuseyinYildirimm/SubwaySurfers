using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class ScoreData
{
    public int maxScore;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [Header("Score")]
    public int score;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI maxScoreText;

    [Header("Coin")]
    [SerializeField] private TextMeshProUGUI coinText;
    public int coin;

    [Space(10)]

    public bool gameOver;
    public static GameManager _instance;

    [SerializeField] private CharacterHealth characterHealth;
    Vector3 characterStartPos;
    float distance;

    private string filePath;


    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        characterStartPos = characterHealth.transform.position;

        filePath = Path.Combine(Application.persistentDataPath, "scoreData.json");
        LoadScore();
    }

    public void Update()
    {
        ScoreControl();

        if (characterHealth.health == 0)
        {
            gameOver = true;
            gameOverText.SetActive(true);
        }
        else
        {
            gameOver = false;
            gameOverText.SetActive(false);
        }
    }

    public void CollectCoin()
    {
        coin++;
        coinText.text = coin.ToString();
    }

    #region SCORE

    void ScoreControl()
    {
        Vector3 characterLastPos = characterHealth.transform.position;

        distance = Vector3.Distance(characterStartPos, characterLastPos);

        score = (int)distance;

        scoreText.text = score.ToString("00000");


        if (gameOver)
        {
            SaveScore(score);
        }
    }

    public void SaveScore(int newScore)
    {
        if (newScore > GetMaxScore())
        {
            ScoreData scoreData = new ScoreData();
            scoreData.maxScore = newScore;

            maxScoreText.text = newScore.ToString("00000");

            string json = JsonUtility.ToJson(scoreData);
            File.WriteAllText(filePath, json);
        }
    }


    public int GetMaxScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);
            return scoreData.maxScore;
        }
        else
        {
            return 0;
        }
    }

    void LoadScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ScoreData scoreData = JsonUtility.FromJson<ScoreData>(json);

            maxScoreText.text = scoreData.maxScore.ToString("00000");
        }
    }

    #endregion  
}

using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Values")]
    [NaughtyAttributes.ReadOnly] [SerializeField] int currentScore;
    [NaughtyAttributes.ReadOnly] [SerializeField] int playerBestScore;
    [NaughtyAttributes.ReadOnly] [SerializeField] int bestScore;

    [Header("Target")]
    [SerializeField] TextMeshPro scoreDisplay;
    [SerializeField] TextMesh gameOverScoreDisplay;
    [SerializeField] TextMesh gameOverPersonalHighScoreDisplay;
    [SerializeField] TextMesh gameOverGlobalHighScoreDisplay;


    //variables for scale up/down de Thomas
    public float minValue;
    public float maxValue;
    public float duration = 1f;

    private float fontSizeValue;
    private bool increasing = true;
    private float timer = 0f;
    private bool interpolationComplete = false;
    private bool scaleUp = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fontSizeValue = minValue;
        if (ScoreData.Instance.playerBestScore != 0) playerBestScore = ScoreData.Instance.playerBestScore;
        if (PlayerPrefs.HasKey("HighScore")) bestScore = PlayerPrefs.GetInt("HighScore");
    }
    
    //scale up de thom, � essayer avec le casque
    void ScaleUpScaleDown()
    {
        if (scaleUp == true)
        {
            interpolationComplete = false;

            timer += Time.deltaTime;

            if (timer >= duration)
            {
                if (!increasing)
                {
                    interpolationComplete = true; // stop updating after reaching maximum value
                    scaleUp = false;
                    timer = 0f;
                }

                timer = 0f;
                increasing = !increasing;
                
            }

            if (!interpolationComplete)
            {
                if (increasing)
                {
                    fontSizeValue = Mathf.Lerp(minValue, maxValue, timer / duration);
                }
                else
                {
                    fontSizeValue = Mathf.Lerp(maxValue, minValue, timer / duration);
                }
            }

            scoreDisplay.fontSize = fontSizeValue;
        }
    }


    void Update()
    {
        ScaleUpScaleDown();
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        scaleUp = true;
        scoreDisplay.text = currentScore.ToString();
    }

    public void SetPlayerBestScore()
    {
        

        if (currentScore < playerBestScore) return;

        playerBestScore = currentScore;
        ScoreData.Instance.playerBestScore = playerBestScore; 
        gameOverPersonalHighScoreDisplay.text = "Personnal Best Score:" + "\n" + playerBestScore.ToString();
    } 

    public void SetBestScore()
    {
        

        if (playerBestScore < bestScore) return;

        bestScore = playerBestScore;
        gameOverGlobalHighScoreDisplay.text = "Best Score:" + "\n" + playerBestScore.ToString();
        
        if (PlayerPrefs.HasKey("HighScore") && PlayerPrefs.GetInt("HighScore") != bestScore) 
        {
            PlayerPrefs.SetInt("HighScore", bestScore);
            PlayerPrefs.Save();
        }
    } 

    public void SetScore(TextMeshPro scoreTextMesh)
    {
        gameOverScoreDisplay.text = "Score:" + "\n" + currentScore.ToString();
    } 

    private void OnDisable() 
    {
        if (PlayerPrefs.HasKey("HighScore") && PlayerPrefs.GetInt("HighScore") != bestScore) 
        {
            PlayerPrefs.SetInt("HighScore", bestScore);
            PlayerPrefs.Save();
        }
    }


}

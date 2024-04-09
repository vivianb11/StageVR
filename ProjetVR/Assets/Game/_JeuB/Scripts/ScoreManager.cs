using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Values")]
    [NaughtyAttributes.ReadOnly] [SerializeField] int currentScore;
    [NaughtyAttributes.ReadOnly] [SerializeField] int playerBestScore;
    [NaughtyAttributes.ReadOnly] [SerializeField] int bestScore;

    [Header("Target")]
    [SerializeField] TextMeshPro scoreDisplay;


    //variables for scale up/down de Thomas
    public float minValue;
    public float maxValue;
    public float duration = 1f;

    private float fontSizeValue;
    private bool increasing = true;
    private float timer = 0f;
    private bool interpolationComplete = false;
    private bool scaleUp = false;


    private void Start()
    {
        fontSizeValue = minValue;
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

    public void SetPlayerBestScore(TextMeshPro playerBestScoreTextMesh)
    {
        if (currentScore < playerBestScore) return;

        playerBestScore = currentScore;
        playerBestScoreTextMesh.text = "Personnal Best Score:" + "\n" + playerBestScore.ToString();
    } 

    public void SetBestScore(TextMeshPro bestScoreTextMesh)
    {
        if (playerBestScore < bestScore) return;

        bestScore = playerBestScore;
        bestScoreTextMesh.text = "Best Score:" + "\n" + playerBestScore.ToString();
    } 

    public void SetScore(TextMeshPro scoreTextMesh)
    {
        scoreTextMesh.text = "Score:" + "\n" + currentScore.ToString();
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

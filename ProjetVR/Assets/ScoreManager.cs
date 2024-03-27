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
    public float minValue = 6f;
    public float maxValue = 9f;
    public float duration = 1f;

    private float currentValue;
    private bool increasing = true;
    private float timer = 0f;
    private bool interpolationComplete = false;
    private bool scaleUp = false;


    private void Start()
    {
        currentValue = minValue;
        if (PlayerPrefs.HasKey("HighScore")) bestScore = PlayerPrefs.GetInt("HighScore");
    }
    
    //scale up de thom, à essayer avec le casque
    void ScaleUpScaleDown()
    {
        if (scaleUp == true)
        {
            if (interpolationComplete)
                return;

            timer += Time.deltaTime;

            if (timer >= duration)
            {
                timer = 0f;
                increasing = !increasing;
                if (!increasing)
                    interpolationComplete = true; // Stop updating after reaching maximum value
            }

            if (!interpolationComplete)
            {
                if (increasing)
                {
                    currentValue = Mathf.Lerp(minValue, maxValue, timer / duration);
                }
                else
                {
                    currentValue = Mathf.Lerp(maxValue, minValue, timer / duration);
                }
            }
            // Use the currentValue for whatever purpose you need
            Debug.Log("Current value: " + currentValue);
            scaleUp = false;
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

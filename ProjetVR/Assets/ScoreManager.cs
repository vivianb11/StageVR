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

    
    private void Start()
    {
        if (PlayerPrefs.HasKey("HighScore")) bestScore = PlayerPrefs.GetInt("HighScore");
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
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

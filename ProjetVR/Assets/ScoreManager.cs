using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using NaughtyAttributes;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{

    [NaughtyAttributes.ReadOnly] [SerializeField] int currentScore;
    [NaughtyAttributes.ReadOnly] [SerializeField] int highScore;

    
    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
    }
}

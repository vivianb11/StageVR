using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyPresets", menuName = "DifficultyPresets", order = 0)]
public class DifficultyPresets : ScriptableObject 
{
    [Header("Difficulty Parameters")]
    public int countCondition;
    public float spawnInterval;
    public float mobSpeed;
    [Range(1, 8)] public int numberSpawner;
    public int weightEnemy1;
    public int weightEnemy2;
    public int weightEnemy3;
    
}


using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyPresets", menuName = "DifficultyPresets", order = 0)]
public class DifficultyPresets : ScriptableObject 
{
    [Header("Difficulty Parameters")]
    public bool levelPassed;
    public int milestoneInterval;
    public int countCondition;
    public float spawnInterval;
    public float mobSpeed;
    public bool isNumberSpawnerBased; 
    [ShowIf("isNumberSpawnerBased")] [Range(1, 6)] public int numberSpawner;
    [HideIf("isNumberSpawnerBased")] public bool spawnerTop;
    [HideIf("isNumberSpawnerBased")] public bool spawnerTopRight;
    [HideIf("isNumberSpawnerBased")] public bool spawnerBottomRight;
    [HideIf("isNumberSpawnerBased")] public bool spawnerBottom;
    [HideIf("isNumberSpawnerBased")] public bool spawnerBottomLeft;
    [HideIf("isNumberSpawnerBased")] public bool spawnerTopLeft;
    [Space(10)]
    public bool weightBased = true; 
    [Space(10)]
    [HideIf("weightBased")] public MobEnum.MobType[] mobTypePerSpawner = new MobEnum.MobType[6];
    [ShowIf("weightBased")] public int weightEnemy1;
    [ShowIf("weightBased")] public int weightEnemy2;
    [ShowIf("weightBased")] public int weightEnemy3;
    
}


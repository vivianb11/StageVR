using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TeethGeneration_", menuName = "ScriptableObject/TeethGeneration", order = 0)]
public class SO_TeethGeneration : ScriptableObject
{
    [Header("Generation Settings")]
    public int numberOfPeices = 10;

    public Anomaly[] anomalies;

    [Range(0, 1)]
    public float smellSpawnChance;

    public int GetTotalPossibleAnomalies()
    {
        int count = 0;
        foreach (var item in anomalies)
        {
            count += item.minMax.x;
        }

        return count;
    }
}

[Serializable]
public struct Anomaly
{
    public TeethState teethState;

    [MinMaxSlider(0, 10)]
    public Vector2Int minMax;

    public AnimationCurve curve;
}

public enum TeethState
{
    Clean,
    Dirty,
    Tartar,
    Decay
}
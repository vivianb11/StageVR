using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeethGeneration_", menuName = "ScriptableObject/TeethGeneration", order = 0)]
public class SO_TeethGrenration : ScriptableObject
{
    [Header("Generation Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxAnomalies;
}

public enum TeethState
{
    Clean,
    Dirty,
    Tartar,
    Decay
}
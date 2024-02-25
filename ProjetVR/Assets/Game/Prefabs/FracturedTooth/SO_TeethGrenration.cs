using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TeethGeneration_", menuName = "ScriptableObject/TeethGeneration", order = 0)]
public class SO_TeethGrenration : ScriptableObject
{
    [Header("Generation Settings")]
    public int numberOfPeices;

    [Space(10)]
    [Range(0, 10)]
    public int minClean;
    [Space(10)]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxAnomalies;
    public AnimationCurve anomalieChance;


    [ShowIf("Dirty")]
    [Header("Dirty Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxDirty;
    [ShowIf("Dirty")]
    public AnimationCurve dirtyChance;

    [ShowIf("Tartar")]
    [Header("Tartar Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxTartar;
    [ShowIf("Tartar")]
    public AnimationCurve tartarChance;

    [ShowIf("Decay")]
    [Header("Decay Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxDecay;
    [ShowIf("Decay")]
    public AnimationCurve decayChance;
}

public enum TeethState
{
    Clean,
    Dirty,
    Tartar,
    Decay
}
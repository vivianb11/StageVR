using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TeethGeneration_", menuName = "ScriptableObject/TeethGeneration", order = 0)]
public class SO_TeethGrenration : ScriptableObject
{
    [Header("Generation Settings")]
    [InfoBox("The number of peices must be a square number", EInfoBoxType.Warning)]
    public int numberOfPeices;

    public bool hasDirty, hasTartar, hasDecay;

    [Space(10)]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxClean;
    public int weightClean;

    [ShowIf("hasDirty")]
    [Header("Dirty Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxDirty;
    [ShowIf("hasDirty")]
    public AnimationCurve dirtyChance;

    [ShowIf("hasTartar")]
    [Header("Tartar Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxTartar;
    [ShowIf("hasTartar")]
    public AnimationCurve tartarChance;

    [ShowIf("hasDecay")]
    [Header("Decay Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxDecay;
    [ShowIf("hasDecay")]
    public AnimationCurve decayChance;

    public List<TeethState> GetActives(bool withClean)
    {
        List<TeethState> actives = new List<TeethState>();

        if (hasDirty)
        {
            actives.Add(TeethState.Dirty);
        }

        if (hasTartar)
        {
            actives.Add(TeethState.Tartar);
        }

        if (hasDecay)
        {
            actives.Add(TeethState.Decay);
        }

        if (withClean)
        {
            actives.Add(TeethState.Clean);
        }

        return actives;
    }
}

public enum TeethState
{
    Clean,
    Dirty,
    Tartar,
    Decay
}
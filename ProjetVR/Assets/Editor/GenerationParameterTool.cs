using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GenerationParameterTool : EditorWindow
{
#if UNITY_EDITOR
    [Header("Generation Settings")]
    public int numberOfPeices;
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxAnomalies;
    public AnimationCurve anomalieChance;

    [Header("Clean Settings")]
    public Vector2Int minMaxClean;
    public AnimationCurve cleanChance;

    [Header("Dirty Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxDirty;
    public AnimationCurve dirtyChance;

    [Header("Tartar Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxTartar;
    public AnimationCurve tartarChance;

    [Header("Decay Settings")]
    [MinMaxSlider(0, 10)]
    public Vector2Int minMaxDecay;
    public AnimationCurve decayChance;

    [MenuItem("Tools/GenerationParameterTool")]
    public static void ShowWindow()
    {
        GetWindow<GenerationParameterTool>("GenerationParameterTool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generation Settings", EditorStyles.boldLabel);
        GUILayout.Space(10);
        numberOfPeices = EditorGUILayout.IntField("Number Of Peices", numberOfPeices);
        GUILayout.Space(10);
        minMaxClean = EditorGUILayout.Vector2IntField("Min Max Clean", minMaxClean);
        GUILayout.Space(10);
        minMaxAnomalies = EditorGUILayout.Vector2IntField("Min Max Anomalies", minMaxAnomalies);
        anomalieChance = EditorGUILayout.CurveField("Anomalie Chance", anomalieChance);


        GUILayout.Label("Dirty Settings", EditorStyles.boldLabel);
        minMaxDirty = EditorGUILayout.Vector2IntField("Min Max Dirty", minMaxDirty);
        dirtyChance = EditorGUILayout.CurveField("Dirty Chance", dirtyChance);

        GUILayout.Label("Tartar Settings", EditorStyles.boldLabel);
        minMaxTartar = EditorGUILayout.Vector2IntField("Min Max Tartar", minMaxTartar);
        tartarChance = EditorGUILayout.CurveField("Tartar Chance", tartarChance);

        GUILayout.Label("Decay Settings", EditorStyles.boldLabel);
        minMaxDecay = EditorGUILayout.Vector2IntField("Min Max Decay", minMaxDecay);
        decayChance = EditorGUILayout.CurveField("Decay Chance", decayChance);

        GUILayout.Space(20);
        // display both buttons on the same line
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
        if (GUILayout.Button("Reset"))
        {
            ResetValues();
        }
        GUILayout.EndHorizontal();
    }

    private void OnSelectionChange()
    {
        SO_TeethGrenration so = Selection.activeObject as SO_TeethGrenration;
        if (so != null)
        {
            numberOfPeices = so.numberOfPeices;

            minMaxClean = so.minMaxClean;

            minMaxDirty = so.minMaxDirty;
            dirtyChance = so.dirtyChance;

            minMaxTartar = so.minMaxTartar;
            tartarChance = so.tartarChance;

            minMaxDecay = so.minMaxDecay;
            decayChance = so.decayChance;
        }
    }

    void Update()
    {
        Repaint();

        minMaxClean = Vector2Clamp(minMaxClean, 0, numberOfPeices);

        // avoids the vector to have the min value higher than the max value
        minMaxAnomalies = Vector2Clamp(minMaxAnomalies, 0, numberOfPeices - minMaxClean.x);
        minMaxDirty = Vector2Clamp(minMaxDirty, 0, numberOfPeices - minMaxClean.x);
        minMaxTartar = Vector2Clamp(minMaxTartar, 0,numberOfPeices - minMaxClean.x);
        minMaxDecay = Vector2Clamp(minMaxDecay, 0,numberOfPeices - minMaxClean.x);

        // avoids the curve to go below 0 or above 1 on both axis
        anomalieChance = CurvClamp(anomalieChance);
        cleanChance = CurvClamp(cleanChance);
        dirtyChance = CurvClamp(dirtyChance);
        tartarChance = CurvClamp(tartarChance);
        decayChance = CurvClamp(decayChance);
    }

    public Vector2Int Vector2Clamp(Vector2Int vector)
    {
        vector.x = Mathf.Clamp(vector.x, 0, vector.y);
        vector.y = Mathf.Clamp(vector.y, vector.x, numberOfPeices);

        return vector;
    }

    public Vector2Int Vector2Clamp(Vector2Int vector, int xMin, int yMax)
    {
        vector.x = Mathf.Clamp(vector.x, xMin, vector.y);
        vector.y = Mathf.Clamp(vector.y, vector.x, yMax);

        return vector;
    }

    public Vector2Int Vector2Clamp(Vector2Int vector, int xMin)
    {
        vector.x = Mathf.Clamp(vector.x, xMin, vector.y);
        vector.y = Mathf.Clamp(vector.y, vector.x, numberOfPeices);

        return vector;
    }

    public AnimationCurve CurvClamp(AnimationCurve curve)
    {
        for (int i = 0; i < curve.length; i++)
        {
            Keyframe key = curve[i];
            key.value = Mathf.Clamp(key.value, 0, 1);
            key.time = Mathf.Clamp(key.time, 0, 1);
            curve.MoveKey(i, key);
        }

        return curve;
    }

    public void ResetValues()
    {
        numberOfPeices = 0;

        minMaxAnomalies = new Vector2Int(0, 0);
        anomalieChance = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        minMaxClean = new Vector2Int(0, 0);
        cleanChance = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        minMaxDirty = new Vector2Int(0, 0);
        dirtyChance = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        minMaxTartar = new Vector2Int(0, 0);
        tartarChance = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        minMaxDecay = new Vector2Int(0, 0);
        decayChance = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
    }

    public void Generate()
    {
        // si un scriptable object de type SO_TeethGrenration est selectionné alors on modifie ses valeurs
        SO_TeethGrenration so = Selection.activeObject as SO_TeethGrenration;
        if (so != null)
        {
            so.numberOfPeices = numberOfPeices;

            so.minMaxClean = minMaxClean;

            so.minMaxDirty = minMaxDirty;
            so.dirtyChance = dirtyChance;

            so.minMaxTartar = minMaxTartar;
            so.tartarChance = tartarChance;

            so.minMaxDecay = minMaxDecay;
            so.decayChance = decayChance;
        }
        else
        {
            so = ScriptableObject.CreateInstance<SO_TeethGrenration>();
            so.numberOfPeices = numberOfPeices;
            so.numberOfPeices = numberOfPeices;

            so.minMaxClean = minMaxClean;

            so.minMaxDirty = minMaxDirty;
            so.dirtyChance = dirtyChance;

            so.minMaxTartar = minMaxTartar;
            so.tartarChance = tartarChance;

            so.minMaxDecay = minMaxDecay;
            so.decayChance = decayChance;

            string path = EditorUtility.SaveFilePanelInProject("Save SO", "TeethGeneration_", "asset", "Please enter a file name to save the SO to");
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = so;
        }
    }
#endif
}

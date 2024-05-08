using UnityEngine;

[CreateAssetMenu(fileName = "New Cell Data", menuName = "ScriptableObject/Cell")]
public class SO_CellData : ScriptableObject
{
    public int maxToothPasteCount = 10;

    public float tartarInteractionTime = 1f;
    public float dirtryInteractionTime = 1f;
}

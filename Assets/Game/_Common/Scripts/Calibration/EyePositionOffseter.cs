using NaughtyAttributes;
using UnityEngine;

public class EyePositionOffseter : MonoBehaviour
{
    [SerializeField] float space;
    [SerializeField] float horizontalRatio = 1f;

    [SerializeField] Transform up;
    [SerializeField] Transform down;

    [SerializeField] Transform left;
    [SerializeField] Transform right;

    [Button]
    public void UpdatePositions()
    {
        up.localPosition = Vector3.up * space;
        down.localPosition = Vector3.down * space;

        left.localPosition = Vector3.left * space * horizontalRatio;
        right.localPosition = Vector3.right * space * horizontalRatio;
    }
}

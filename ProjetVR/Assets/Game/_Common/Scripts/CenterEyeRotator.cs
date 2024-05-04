using NaughtyAttributes;
using UnityEngine;

public class CenterEyeRotator : MonoBehaviour
{
    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightEye;

    [SerializeField] bool useSimpleOffset = true;

    [ShowIf("useSimpleOffset")]
    [SerializeField] Vector2 eyeOffset = new Vector2(0, 0);

    [HideIf("useSimpleOffset")]
    [SerializeField] Vector2 LeftOffset = new Vector2(0, 0);
    [HideIf("useSimpleOffset")]
    [SerializeField] Vector2 RightOffset = new Vector2(0, 0);

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(leftEye.rotation, rightEye.rotation, 0.5f);

        if (useSimpleOffset)
        {
            // Apply eye offset to the rotation
            transform.rotation = Quaternion.Euler(eyeOffset) * transform.rotation;
        }
        else
        {
            // looks at the left eye rotation on the y axis and interpolates 0 if at -75 degrees and 1 if at 75 degrees
            float lerpValue = Mathf.InverseLerp(-75, 75, leftEye.localEulerAngles.y);
            // Apply the offset to the rotation
            transform.rotation = Quaternion.Euler(Vector2.Lerp(LeftOffset, RightOffset, lerpValue)) * transform.rotation;
        }
    }
}

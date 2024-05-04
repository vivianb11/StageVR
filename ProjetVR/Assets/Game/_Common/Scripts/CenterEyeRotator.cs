using UnityEngine;

public class CenterEyeRotator : MonoBehaviour
{
    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightEye;

    [SerializeField] Vector2 eyeOffset = new Vector2(0, 0);

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(leftEye.rotation, rightEye.rotation, 0.5f);

        // Apply eye offset to the rotation
        transform.rotation = Quaternion.Euler(eyeOffset) * transform.rotation;
    }
}

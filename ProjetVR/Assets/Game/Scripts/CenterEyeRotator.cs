using UnityEngine;

public class CenterEyeRotator : MonoBehaviour
{
    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightEye;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(leftEye.rotation, rightEye.rotation, 0.5f);
    }
}

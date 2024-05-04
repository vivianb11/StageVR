using UnityEngine;

public class CenterEyeRotator : MonoBehaviour
{
    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightEye;

    [SerializeField] Vector2 eyeOffset = new Vector2(0, 0);

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(leftEye.position, rightEye.position, 0.5f) + new Vector3(eyeOffset.x, eyeOffset.y, 0);
    }
}

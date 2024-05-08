using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] float lerpSpeed = 0.5f;

    [SerializeField] Vector3 eulerAngleOffset;

    [SerializeField] Transform targetTransform;

    private void FixedUpdate()
    {
        transform.LookAt(targetTransform);
        transform.eulerAngles += eulerAngleOffset;
    }

    public void SetTarget(Transform newTarget)
    {
        targetTransform = newTarget;
    }
}

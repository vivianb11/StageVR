using UnityEngine;

public class LookAt : MonoBehaviour
{
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

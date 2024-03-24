using NaughtyAttributes;
using UnityEngine;

public class ShieldDetection : MonoBehaviour
{
    [SerializeField] [ReadOnly] private GameObject currentActiveShield;

    [SerializeField] float minDetectionRange = 1.0f;

    [SerializeField] Transform[] points;

    void Start()
    {
        currentActiveShield = null;
    }

    private void FixedUpdate()
    {
        Vector3 hitPosition = EyeManager.Instance.hitPosition;
        float distanceToHitpoint = Vector3.Distance(transform.position, hitPosition);

        if (distanceToHitpoint < minDetectionRange)
            return;

        Transform closestPoint = points[0];
        float closestDistance = Vector3.Distance(closestPoint.position, hitPosition);

        foreach (Transform t in points)
        {
            float distance = Vector3.Distance(t.position, hitPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = t;
            }
        }

        Activate(closestPoint);
    }

    public void Activate(GameObject obj)
    {
        DeactivateCurrentShield();

        obj.SetActive(true);

        currentActiveShield = obj;
    }

    public void Activate(Transform obj)
    {
        DeactivateCurrentShield();

        obj.gameObject.SetActive(true);

        currentActiveShield = obj.gameObject;
    }

    private void DeactivateCurrentShield()
    {
        if (currentActiveShield != null && currentActiveShield.activeSelf)
        {
            currentActiveShield.SetActive(false);
        }
    }
}

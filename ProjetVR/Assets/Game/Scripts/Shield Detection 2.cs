using NaughtyAttributes;
using UnityEngine;

public class ShieldDetection2 : MonoBehaviour
{
    public GameObject shield; 

    [SerializeField]
    private float rotationSpeed = 2.0f; 

    void Update()
    {
        Vector3 hitPosition = EyeManager.Instance.hitPosition;

        if (shield != null)
        {
            Vector3 targetDirection = hitPosition - shield.transform.position;
            float singleStep = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(shield.transform.forward, targetDirection, singleStep, 0.0f);
            shield.transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
using UnityEngine;

public class SmoothShield : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(EyeManager.Instance.hitPosition);

        Vector3 rot = transform.localEulerAngles;
        rot.x = 0;
        rot.z = 0;

        transform.localEulerAngles = rot;
    }
}

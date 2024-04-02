using UnityEngine;

public class CameraAlignView : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float lerpSpeed;
    [SerializeField] float offset = 10f;

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTransform.position + cameraTransform.forward * offset, lerpSpeed);
    }
}

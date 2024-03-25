using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool snaping = true;

    [SerializeField] int snapValue = 90;
    [SerializeField] Transform trakinckPoint;
    [SerializeField] AnimationCurve curve;

    private Transform cameraTarget;

    public void Start()
    {
        cameraTarget = Camera.main.transform;

        transform.LookAt(cameraTarget.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public void RotateTowards(RotatorController rotatorController)
    {
        Vector3 direction = Vector3.zero;

        switch (rotatorController.direction)
        {
            case RotatorController.RotationDirection.UP:
                direction = trakinckPoint.right;
                break;
            case RotatorController.RotationDirection.DOWN:
                direction = -trakinckPoint.right;
                break;
            case RotatorController.RotationDirection.LEFT:
                direction = trakinckPoint.up;
                break;
            case RotatorController.RotationDirection.RIGHT:
                direction = -trakinckPoint.up;
                break;
        }

        if (snaping)
        {
            StartCoroutine(SmoothRotate(transform, direction, snapValue));
        }
        else
        {
            StartCoroutine(SmoothRotate(transform, direction, 1));
        }
    }

    public void SetAsChild(Transform objectTransform)
    {
        objectTransform.SetParent(transform, true);
        objectTransform.localPosition = Vector3.zero;
    }

    public void YeetTheCild()
    {
        transform.GetChild(0).transform.SetParent(null);
    }

    public void ResetRotator()
    {
        StopAllCoroutines();

        transform.LookAt(cameraTarget.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private IEnumerator SmoothRotate(Transform target, Vector3 axis, float angle)
    {
        float elapsedTime = 0;
        float duration = 0.5f; // Adjust the duration as needed for the desired rotation speed

        Quaternion startRotation = target.rotation;
        Quaternion endRotation = Quaternion.AngleAxis(angle, axis) * startRotation;

        while (elapsedTime < duration)
        {
            target.rotation = Quaternion.Slerp(startRotation, endRotation, curve.Evaluate(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation; // Ensure the final rotation is exactly as desired
    }
}

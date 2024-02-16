using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool snaping = true;

    public GameObject rotationControleurs;

    public float controlerSpacing = 5f;

    [SerializeField]
    private int snapValue = 90;

    public void Awake()
    {
        Camera camera = Camera.main;

        this.transform.LookAt(camera.transform);

        GameObject go = Instantiate(rotationControleurs);
        go.transform.position = this.transform.position + this.transform.up * controlerSpacing;
        go.transform.rotation = this.transform.rotation;
        go.transform.Rotate(0, 0, 90);
        SetupInteractable(go);


        go = Instantiate(rotationControleurs);
        go.transform.position = this.transform.position - this.transform.up * controlerSpacing;
        go.transform.rotation = this.transform.rotation;
        go.transform.Rotate(0, 0, -90);
        SetupInteractable(go);

        go = Instantiate(rotationControleurs);
        go.transform.position = this.transform.position + this.transform.right * controlerSpacing;
        go.transform.rotation = this.transform.rotation;
        go.transform.Rotate(0, 90, 0);
        SetupInteractable(go);

        go = Instantiate(rotationControleurs);
        go.transform.position = this.transform.position - this.transform.right * controlerSpacing;
        go.transform.rotation = this.transform.rotation;
        go.transform.Rotate(0, 0, 180);
        SetupInteractable(go);

    }

    private void SetupInteractable(GameObject go)
    {
        Interactable inter = go.GetComponent<Interactable>();
        inter.onSelected.AddListener(() => RotateTowards(go));
    }

    public void RotateTowards(GameObject gameObject)
    {
        Vector3 direction = gameObject.transform.up;

        if (snaping)
        {
            StartCoroutine(SmoothRotate(this.transform, direction, snapValue));
        }
        else
        {
            StartCoroutine(SmoothRotate(this.transform, direction, 1));
        }
    }

    private IEnumerator SmoothRotate(Transform target, Vector3 axis, float angle)
    {
        float elapsedTime = 0;
        float duration = 0.5f; // Adjust the duration as needed for the desired rotation speed

        Quaternion startRotation = target.rotation;
        Quaternion endRotation = Quaternion.AngleAxis(angle, axis) * startRotation;

        while (elapsedTime < duration)
        {
            target.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation; // Ensure the final rotation is exactly as desired
    }
}

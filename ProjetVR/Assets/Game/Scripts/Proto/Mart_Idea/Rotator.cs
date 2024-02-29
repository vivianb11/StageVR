using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool snaping = true;

    public GameObject rotationControleurs;

    public float controlerSpacing = 5f;
    
    [SerializeField]
    private int snapValue = 90;

    private Transform camera;

    public void Awake()
    {
        camera = Camera.main.transform;

        transform.LookAt(camera.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);

        GameObject go = Instantiate(rotationControleurs);
        go.transform.position = transform.position + transform.up * controlerSpacing;
        go.transform.rotation = transform.rotation;
        go.transform.Rotate(0, 0, 90);
        SetupInteractable(go);


        go = Instantiate(rotationControleurs);
        go.transform.position = transform.position - transform.up * controlerSpacing;
        go.transform.rotation = transform.rotation;
        go.transform.Rotate(0, 0, -90);
        SetupInteractable(go);

        go = Instantiate(rotationControleurs);
        go.transform.position = transform.position + transform.right * controlerSpacing;
        go.transform.rotation = transform.rotation;
        go.transform.Rotate(0, 90, 0);
        SetupInteractable(go);

        go = Instantiate(rotationControleurs);
        go.transform.position = transform.position - transform.right * controlerSpacing;
        go.transform.rotation = transform.rotation;
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

        transform.LookAt(camera.position);
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
            target.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation; // Ensure the final rotation is exactly as desired
    }
}

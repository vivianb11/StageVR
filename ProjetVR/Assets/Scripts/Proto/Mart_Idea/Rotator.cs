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
        inter.selectConditions[0].conditionValue = 1f;
        inter.deselectConditions[0].conditionValue = 0f;
        inter.onSelected.AddListener(() => RotateTowards(go));
    }

    public void RotateTowards(GameObject gameObject)
    {
        Vector3 direction = gameObject.transform.up;
        
        if (snaping)
            this.transform.RotateAround(this.transform.position, direction, snapValue);
        else
            this.transform.RotateAround(this.transform.position, direction, 1);

    }
}

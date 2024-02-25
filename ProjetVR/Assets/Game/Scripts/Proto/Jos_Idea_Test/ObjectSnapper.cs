using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class ObjectSnapper : MonoBehaviour
{
    private Material material;
    private bool isSnapped = false;
    private Interactable snappedObject;
    private SphereCollider sphereCollider;

    [Space(10)]
    public bool interactableAfterSnapped = false;

    [Foldout("Events")]
    public UnityEvent<Transform> onSnapped;
    [Foldout("Events")]
    public UnityEvent onUnsnapped;

    [Space(10)]
    public float detectionRadius = 0.1f;
    public float snapDistance = 0.1f;

    public List<GameObject> snappableObjects = new();

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = detectionRadius;

        this.transform.localScale += new Vector3(0.1f,0.1f,0.1f);

        if (TryGetComponent(out MeshRenderer mehs))
            material = mehs.material;
        else if (GetComponentInChildren<MeshRenderer>())
            material = GetComponentInChildren<MeshRenderer>().material;

        if (material)
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
    }

    void FixedUpdate()
    {
        if (isSnapped && Vector3.Distance(snappedObject.transform.position, transform.position) > snapDistance)
        {
            isSnapped = false;
            onUnsnapped.Invoke();

            snappedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSnapped)
            return;

        if (!snappableObjects.Contains(other.gameObject))
            return;
        
        if (other.TryGetComponent(out Interactable interacable))
        {
            onSnapped?.Invoke(interacable.transform);
            interacable.DeSelect();

            interacable.rb.isKinematic = true;

            interacable.transform.position = transform.position;
            interacable.transform.rotation = transform.rotation;

            isSnapped = true;
            snappedObject = interacable;

            if (!interactableAfterSnapped)
                interacable.SetCanBeInteracted(false);
        }
    }
}

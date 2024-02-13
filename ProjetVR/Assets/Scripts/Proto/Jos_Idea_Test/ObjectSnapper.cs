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
    private GameObject snappedObject;
    private SphereCollider sphereCollider;

    [Space(10)]
    public bool interactableAfterSnapped = false;

    [Foldout("Events")]
    public UnityEvent onSnapped;
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

        material = GetComponent<MeshRenderer>().material;

        material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
    }

    void FixedUpdate()
    {
        if (isSnapped && Vector3.Distance(snappedObject.transform.position, transform.position) > snapDistance)
        {
            isSnapped = false;
            onUnsnapped.Invoke();
            
            if (snappedObject.TryGetComponent(out Interactable interacable))
            {
                interacable.DeInteract();
            }
            
            snappedObject = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (GameObject gameObject in snappableObjects)
        {
            if(gameObject == other.gameObject && Vector3.Distance(other.transform.position, transform.position) < snapDistance)
            {
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;

                onSnapped?.Invoke();

                if (!interactableAfterSnapped)
                {
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    other.GetComponent<Interactable>().enabled = false;
                }
                else
                {
                    isSnapped = true;
                    snappedObject = other.gameObject;
                }
            }
        }
    }
}

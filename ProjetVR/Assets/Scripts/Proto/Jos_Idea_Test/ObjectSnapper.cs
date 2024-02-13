using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSnapper : MonoBehaviour
{
    private Material material;

    public bool interactableAfterSnapped = false;

    UnityEvent onSnapped;

    public float snapDistance = 0.1f;

    public List<GameObject> snappableObjects = new();

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;

        // keep the color of the old material & add transparency
        material.color = new Color(material.color.r, material.color.g, material.color.b, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (snappableObjects.Count > 0)
        {
            for (int i = 0; i < snappableObjects.Count; i++)
            {
                if (Vector3.Distance(transform.position, snappableObjects[i].transform.position) < snapDistance)
                {
                    snappableObjects[i].transform.position = transform.position;
                    snappableObjects[i].transform.rotation = transform.rotation;
                    
                    onSnapped?.Invoke();

                    if (!interactableAfterSnapped)
                    {
                        snappableObjects[i].GetComponent<Rigidbody>().isKinematic = true;
                        snappableObjects[i].GetComponent<Interacable>().enabled = false;
                    }
                }
            }
        }
    }
}

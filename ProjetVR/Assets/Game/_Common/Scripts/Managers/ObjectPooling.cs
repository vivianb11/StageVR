using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Pooling;

    //private List<GameObject> objects = new List<GameObject>();

    private Dictionary<string, List<GameObject>> objects = new ();

    private void Awake()
    {
        Pooling = this;
    }

    public GameObject InstantiateGameObject(GameObject objectToInstantiate)
    {
        GameObject obj = GetActiveObject(objectToInstantiate);

        if (obj == null)
            obj = InstantiateNewObject(objectToInstantiate);

        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        return obj;
    }

    public GameObject InstantiateGameObject(GameObject objectToInstantiate, Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetActiveObject(objectToInstantiate);

        if (obj == null)
            obj = InstantiateNewObject(objectToInstantiate, position, rotation);

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public GameObject InstantiateGameObject(GameObject objectToInstantiate, Transform targetTransform)
    {
        GameObject obj = GetActiveObject(objectToInstantiate);

        if (obj == null)
            obj = InstantiateNewObject(objectToInstantiate, targetTransform);

        obj.transform.SetParent(targetTransform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        return obj;
    }

    private GameObject GetActiveObject(GameObject objectToInstantiate)
    {
        Debug.Log("Object to spawn type: " + objectToInstantiate.name);

        if (!objects.ContainsKey(objectToInstantiate.name))
            return null;

        foreach (GameObject obj in objects[objectToInstantiate.name])
        {
            if (!obj.activeInHierarchy) 
                return obj;
        }

        return null;
    }

    private GameObject InstantiateNewObject(GameObject objectToInstantiate)
    {
        GameObject obj = Instantiate(objectToInstantiate);

        if (!objects.ContainsKey(objectToInstantiate.name))
            objects.Add(objectToInstantiate.name, new List<GameObject>() { obj });
        else
            objects[objectToInstantiate.name].Add(obj);

        return obj;
    }

    private GameObject InstantiateNewObject(GameObject objectToInstantiate, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(objectToInstantiate, position, rotation);

        if (!objects.ContainsKey(objectToInstantiate.name))
            objects.Add(objectToInstantiate.name, new List<GameObject>() { obj });
        else
            objects[objectToInstantiate.name].Add(obj);

        return obj;
    }

    private GameObject InstantiateNewObject(GameObject objectToInstantiate, Transform targetTransform)
    {
        GameObject obj = Instantiate(objectToInstantiate, targetTransform);

        if (!objects.ContainsKey(objectToInstantiate.name))
            objects.Add(objectToInstantiate.name, new List<GameObject>() { obj });
        else
            objects[objectToInstantiate.name].Add(obj);

        return obj;
    }
}

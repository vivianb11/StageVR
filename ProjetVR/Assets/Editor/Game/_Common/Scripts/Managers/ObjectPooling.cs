using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;

    private Dictionary<string, List<GameObject>> objects = new ();

    private void Awake()
    {
        Instance = this;
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
        if (!objects.ContainsKey(objectToInstantiate.name))
            return null;

        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject obj in objects[objectToInstantiate.name])
        {
            if (obj == null)
            {
                objectsToRemove.Add(obj);
                continue;
            }

            if (!obj.activeInHierarchy) 
                return obj;
        }

        foreach (GameObject obj in objectsToRemove)
        {
            objects[objectToInstantiate.name].Remove(obj);
        }

        return null;
    }

    private GameObject InstantiateNewObject(GameObject objectToInstantiate)
    {
        GameObject obj = Instantiate(objectToInstantiate, transform);

        if (!objects.ContainsKey(objectToInstantiate.name))
            objects.Add(objectToInstantiate.name, new List<GameObject>() { obj });
        else
            objects[objectToInstantiate.name].Add(obj);

        return obj;
    }

    private GameObject InstantiateNewObject(GameObject objectToInstantiate, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(objectToInstantiate, position, rotation, transform);

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

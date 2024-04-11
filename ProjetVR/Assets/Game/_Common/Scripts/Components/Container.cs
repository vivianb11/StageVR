using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Container : MonoBehaviour
{
    public enum AlignPosition
    {
        LEFT, CENTER, RIGHT
    }

    public float space;
    public AlignPosition alignPosition;

    private void Start()
    {
        UpdateChildsPosition();
    }

    private void OnEnable()
    {
        UpdateChildsPosition();
    }

    private void OnTransformChildrenChanged()
    {
        UpdateChildsPosition();
    }

    private Transform[] GetActiveChilds()
    {
        List<Transform> activeChilds = new();

        foreach (Transform item in transform)
        {
            if (item.gameObject.activeInHierarchy) activeChilds.Add(item);
        }

        return activeChilds.ToArray();
    }

    [Button]
    private void UpdateChildsPosition()
    {
        Transform[] activeChilds = GetActiveChilds();

        for (int i = 0; i < activeChilds.Length; i++)
        {
            Transform child = activeChilds[i];

            switch (alignPosition)
            {
                case AlignPosition.LEFT:
                    child.localPosition = new Vector3(space, 0, 0) * i;
                    break;
                case AlignPosition.CENTER:
                    child.localPosition = (new Vector3(space, 0, 0) * i) - new Vector3(space * (activeChilds.Length - 1), 0, 0) / 2f;
                    break;
                case AlignPosition.RIGHT:
                    child.localPosition = new Vector3(-space, 0, 0) * i;
                    break;
            }
        }
    }
}

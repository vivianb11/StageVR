using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Container : MonoBehaviour
{
    public enum AlignDirection
    {
        VERTICAL, HORIZONTAL
    }

    public enum AlignPosition
    {
        LEFT, CENTER, RIGHT
    }

    public float space = 1f;

    public AlignPosition alignPosition = AlignPosition.CENTER;
    public AlignDirection alignDirection = AlignDirection.HORIZONTAL;

    private void Start()
    {
        UpdateChildsPosition();

        foreach (Transform child in transform)
        {
            child.GetComponent<ActiveEvent>().OnActivated.AddListener(UpdateChildsPosition);
        }
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
        List<Transform> activeChilds = GetActiveChilds().ToList();

        Vector3 direction = Vector3.zero;

        switch (alignDirection)
        {
            case AlignDirection.VERTICAL:
                direction = Vector3.up;
                break;
            case AlignDirection.HORIZONTAL:
                direction = Vector3.right;
                break;
        }

        for (int i = 0; i < activeChilds.Count; i++)
        {
            Transform child = activeChilds[i];

            switch (alignPosition)
            {
                case AlignPosition.LEFT:
                    child.localPosition = direction * space * i;
                    break;
                case AlignPosition.CENTER:
                    child.localPosition = (direction * space * i) - direction * (space * (activeChilds.Count - 1)) / 2f;
                    break;
                case AlignPosition.RIGHT:
                    child.localPosition = direction * -space * i;
                    break;
            }
        }
    }
}

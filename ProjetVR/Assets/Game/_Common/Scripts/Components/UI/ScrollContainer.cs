using Components.Container;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollContainer : Container3D
{
    private Vector3 offset;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Navigatable navigatable))
            {
                navigatable.focusEntered.AddListener(OnNavigatableFocusEntered);
            }
        }
    }

    public void OnNavigatableFocusEntered(Navigatable navigatable)
    {
        offset = Vector3.zero;
        UpdateChildsPosition();
        offset = -navigatable.transform.localPosition;
        UpdateChildsPosition();
    }

    protected override void UpdateChildsPosition()
    {
        List<Transform> activeChilds = GetActiveChilds().ToList();
        activeChilds.Reverse();

        Vector3 direction = Vector3.zero;

        switch (alignDirection)
        {
            case AlignDirection.VERTICAL:
                direction = transform.up;
                break;
            case AlignDirection.HORIZONTAL:
                direction = transform.right;
                break;
        }

        for (int i = 0; i < activeChilds.Count; i++)
        {
            Transform child = activeChilds[i];

            switch (alignPosition)
            {
                case AlignPosition.LEFT:
                    child.localPosition = offset + direction * space * i;
                    break;
                case AlignPosition.CENTER:
                    child.localPosition = offset + (direction * space * i) - direction * (space * (activeChilds.Count - 1)) / 2f;
                    break;
                case AlignPosition.RIGHT:
                    child.localPosition = offset + direction * -space * i;
                    break;
            }
        }
    }
}

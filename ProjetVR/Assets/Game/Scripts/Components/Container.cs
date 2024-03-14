using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Container;

[ExecuteInEditMode]
public class Container : MonoBehaviour
{
    public enum AlignPosition
    {
        LEFT, CENTER, RIGHT
    }

    public float space;
    public AlignPosition alignPosition;

#if UNITY_EDITOR
    private void Update()
    {
        UpdateChildsPosition();
    }
#endif

    private void UpdateChildsPosition()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            switch (alignPosition)
            {
                case AlignPosition.LEFT:
                    child.localPosition = new Vector3(space, 0, 0) * i;
                    break;
                case AlignPosition.CENTER:
                    child.localPosition = (new Vector3(space, 0, 0) * i) - new Vector3(space * (transform.childCount - 1), 0, 0) / 2f;
                    break;
                case AlignPosition.RIGHT:
                    child.localPosition = new Vector3(-space, 0, 0) * i;
                    break;
            }
        }
    }
}

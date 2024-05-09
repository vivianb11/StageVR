using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components.Container
{
    public class Container3D : MonoBehaviour
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

        private int _childCount;

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
            if (_childCount != transform.childCount)
            {
                _childCount = transform.childCount;
                StartCoroutine(MoveChildPos());
            }
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
        public void UpdateChildsPosition()
        {
            List<Transform> activeChilds = GetActiveChilds().ToList();

            Debug.Log(activeChilds.Count);

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

        private IEnumerator MoveChildPos()
        {
            yield return new WaitForEndOfFrame();
            UpdateChildsPosition();
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace Nodes
{
    public class Node : MonoBehaviour
    {
        public UnityEvent<Transform> childAdded;

        public UnityEvent<Transform> childRemoved;

        public Node parent;

        public void AddChild(Node newChild)
        {
            Transform newChildTransform = newChild.transform;

            if (newChildTransform.IsChildOf(transform) || newChild == transform)
            {
                Debug.LogError($"Object {newChild.name} is already child of {name}");
                return;
            }

            newChildTransform.SetParent(transform);
            newChild.parent = this;

            childAdded?.Invoke(newChildTransform);
        }

        public void RemoveChild(Node oldChild)
        {
            Transform oldChildTransform = oldChild.transform;

            if (!oldChildTransform.IsChildOf(transform))
            {
                Debug.LogError($"Object {oldChild.name} not child of {name}");
                return;
            }

            oldChildTransform.SetParent(null);
            oldChild.parent = null;

            childRemoved?.Invoke(oldChildTransform);
        }

        public void ReparentTo(Node newParent)
        {
            parent.RemoveChild(this);
            newParent.AddChild(this);
        }
    }
}

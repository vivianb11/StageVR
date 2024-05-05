using UnityEngine;
using UnityEngine.Events;

namespace Nodes
{
    public class Node : MonoBehaviour
    {
        public UnityEvent<Node> childAdded;

        public UnityEvent<Node> childRemoved;

        public Node parent;

        public void AddChild(Node newChild)
        {
            if (newChild.transform.IsChildOf(transform) || newChild == transform)
            {
                Debug.LogError($"Object {newChild.name} is already child of {name}");
                return;
            }

            newChild.transform.SetParent(transform);
            newChild.parent = this;

            childAdded?.Invoke(newChild);
        }

        public void RemoveChild(Node oldChild)
        {
            if (!oldChild.transform.IsChildOf(transform))
            {
                Debug.LogError($"Object {oldChild.name} not child of {name}");
                return;
            }

            oldChild.transform.SetParent(null);
            oldChild.parent = null;

            childRemoved?.Invoke(oldChild);
        }

        public void ReparentTo(Node newParent)
        {
            parent.RemoveChild(this);
            newParent.AddChild(this);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace Nodes
{
    public class UnityNode : MonoBehaviour
    {
        public UnityEvent<UnityNode> childAdded;

        public UnityEvent<UnityNode> childRemoved;

        public UnityNode parent;

        public void AddChild(UnityNode newChild)
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

        public void RemoveChild(UnityNode oldChild)
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

        public void ReparentTo(UnityNode newParent)
        {
            parent.RemoveChild(this);
            newParent.AddChild(this);
        }
    }
}

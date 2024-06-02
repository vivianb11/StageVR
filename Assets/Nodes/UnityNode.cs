using UnityEngine;
using UnityEngine.Events;

namespace Nodes
{
    public class UnityNode : MonoBehaviour
    {
        public UnityEvent<UnityNode> childAdded = new();

        public UnityEvent<UnityNode> childRemoved = new();

        public UnityEvent childOrderChanged = new();

        public UnityEvent destroyed = new();

        public UnityNode parent { get; private set; }

        private void OnEnable()
        {
            parent?.childOrderChanged?.Invoke();
        }

        private void OnDisable()
        {
            parent?.childOrderChanged?.Invoke();
        }

        private void Start()
        {
            parent = transform.parent?.GetComponent<UnityNode>();
        }

        private void OnDestroy()
        {
            destroyed?.Invoke();
            parent?.childOrderChanged?.Invoke();
        }

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
            childOrderChanged?.Invoke();
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
            childOrderChanged?.Invoke();
        }

        public void ReparentTo(UnityNode newParent)
        {
            parent.RemoveChild(this);
            newParent.AddChild(this);
        }
    }
}

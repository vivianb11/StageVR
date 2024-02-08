using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Interacable : MonoBehaviour
{
    [Foldout("Events")]
    public UnityEvent selected;
    [Foldout("Events")]
    public UnityEvent deSelected;

    public virtual void Interact() { }

    public void Select()
    {
        selected?.Invoke();
    }

    public void DeSelect()
    {
        deSelected?.Invoke();
    }
}
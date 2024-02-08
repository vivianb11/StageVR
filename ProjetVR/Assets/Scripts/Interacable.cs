using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Interacable : MonoBehaviour
{
    [Foldout("Events")]
    public UnityEvent select;
    [Foldout("Events")]
    public UnityEvent deSelect;

    public void Select()
    {
        select?.Invoke();
    }

    public void DeSelect()
    {
        deSelect?.Invoke();
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Interacable : MonoBehaviour
{
    public UnityEvent select;
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
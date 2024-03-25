using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(MaterialChanger))]
public class ProtectedToothBehaviours : MonoBehaviour
{
    [Header("Tooth Characteristics")]
    [SerializeField] int health;
    [SerializeField] int receivedDamagedOnHit;
    [SerializeField] UnityEvent onDamaged = new UnityEvent();
    private MaterialChanger _materialChanger;
    private int materialCurrentIndex = 0;

    private void Start()
    {
        _materialChanger = GetComponent<MaterialChanger>();
    }

    public void Damaged()
    {
        if (health <= 0) return;
        onDamaged.Invoke();
        health -= receivedDamagedOnHit;
        materialCurrentIndex += 1;
        Debug.Log(materialCurrentIndex);
        _materialChanger.ChangeMaterial(materialCurrentIndex);
    }
    
}

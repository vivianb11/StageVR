using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ProtectedToothBehaviours : MonoBehaviour
{
    [Header("Tooth Characteristics")]
    [SerializeField] int health;
    [SerializeField] int receivedDamagedOnHit;
    [SerializeField] UnityEvent onDamaged = new UnityEvent();
    [SerializeField] UnityEvent onDeath = new UnityEvent();
    [SerializeField] Material material2HP; //the material the tooth has at 2 hp
    [SerializeField] Material material1HP; //the material the tooth has at 1 hp


    public void Damaged()
    {
        if (health <= 0) return;
        onDamaged.Invoke();
        health -= receivedDamagedOnHit;
        materialChange();
        IsDeadCheck();
    }


    private void materialChange()
    {
        if (health == 2)
        {
            gameObject.GetComponent<Renderer>().material = material2HP;
        }

        if (health == 1)
        {
            gameObject.GetComponent<Renderer>().material = material1HP;
        }
    }

    private bool IsDeadCheck()
    {
        bool condition = health == 0;
        if (condition)
        {
            onDeath.Invoke();
            Destroy(gameObject);
        }
        return condition;
    }

    
}

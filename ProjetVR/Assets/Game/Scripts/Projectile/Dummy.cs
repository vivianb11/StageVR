using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    public int health;

    public void ReceiveDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}

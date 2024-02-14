using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    public int health;

    public void ReceiveDamage(int damage)
    {
        health -= damage;

        if (health <= 0 )
            Destroy(gameObject);
    }
}

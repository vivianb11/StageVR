using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Projectil : MonoBehaviour
{
    public float projectilSpeed;
    public int damage;

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        body.AddForce(transform.forward * projectilSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable component))
            component.ReceiveDamage(damage);
        
        Destroy(gameObject);
    }
}

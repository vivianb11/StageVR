using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Mob : MonoBehaviour
{
    [Header("Mob Characteristics")]
    public Transform target;
    [SerializeField] int lifePoints;
    [SerializeField] float moveSpeed;
    [SerializeField] bool isKnockable;


    [Header("On Hit Characteristics")]
    [SerializeField] int damagedTakenOnHit;
    [SerializeField] float pushForce;
    [SerializeField] float knockDuration;


    private bool _isKnocked = false;
    private Vector3 _spawnDirection;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _spawnDirection = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        IsDead(lifePoints);
    }

    private void OnTriggerEnter(Collider shield)
    {
        if (!shield.gameObject.CompareTag("Shield")) return;
        Damaged();
    }

    private IEnumerator Knocked()
    /*Set player state to knocked and add given force towards spawn direction (invert of moving direction), then wait x seconds and disable knowked state*/
    {
        _isKnocked = true;

        if (_rigidbody != null) _rigidbody.AddForce(_spawnDirection * pushForce, ForceMode.Impulse);

        yield return new WaitForSeconds(knockDuration);
        _isKnocked = false;

    }

    private void Move()
    /*Make the Mob lookAt is target and translate toward this direction by an amount (if not knocked)*/
    {
        transform.LookAt(target.position);
        if (!_isKnocked) transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private bool IsDead(int _life)
    /*Check is Mob life is 0, if so destroy the gameObhect and return bool*/
    {
        var deathCheck = _life == 0;
        if (deathCheck) Destroy(this.gameObject);
        return deathCheck;
    }

    private void Damaged()
    /*Remove x life points and start the knock function if mob is knockable*/
    {
        lifePoints -= damagedTakenOnHit;
        if (isKnockable) StartCoroutine(Knocked());
    }

}

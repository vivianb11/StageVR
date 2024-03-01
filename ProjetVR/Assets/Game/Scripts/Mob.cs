using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Mob : MonoBehaviour
{
    public Transform target;
    public int LifePoints = 1;
    public float moveSpeed = 5f;
    public bool EnemyPush = false;
    public float pushForce = 10f;
    private bool _isKnocked = false;
    Vector3 spawnDirection;

    void Start()
    {
        spawnDirection = transform.position;
    }
    void Update()
    {
        transform.LookAt(target.position);

        if (!_isKnocked)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (LifePoints == 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider shield)
    {
        if (shield.gameObject.CompareTag("Shield"))
        {
            LifePoints -= 1;

            if (EnemyPush)
            {
                StartCoroutine(Knocked());
            }
        }
    }
    private IEnumerator Knocked()
    {
        _isKnocked = true;
        Rigidbody enemyRigidbody = GetComponent<Rigidbody>();
        if (enemyRigidbody != null)
        {
            enemyRigidbody.AddForce(spawnDirection * pushForce, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(2f);
        _isKnocked = false;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Mob : MonoBehaviour
{
<<<<<<< Updated upstream
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
=======

    [HideInInspector] public Transform target;
    public int[] degree;
    public int[] speed;
    private int choice;
    public int LifePoints = 1;
    public float moveSpeed = 5f;
    public float pushBackTime;

    public bool EnemyPush = false;
    public bool _rotate = false;

    private bool _isKnocked = false;

    private void Start()
    {
        if (_rotate)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            StartCoroutine(DelayRotation());
        }
>>>>>>> Stashed changes
    }

    private void Update()
    {
<<<<<<< Updated upstream
        Move();
        IsDead(lifePoints);
=======
        transform.LookAt(target.position);

        if (!_isKnocked)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        else if (_isKnocked && EnemyPush)
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (LifePoints == 0)
        {
            if (transform.parent)
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
>>>>>>> Stashed changes
    }

    private void OnTriggerEnter(Collider shield)
    {
<<<<<<< Updated upstream
        if (!shield.gameObject.CompareTag("Shield")) return;
        Damaged();
    }

=======
        if (shield.gameObject.CompareTag("Shield"))
        {
            LifePoints -= 1;

            if (EnemyPush)
                StartCoroutine(Knocked());
        }

    }

    private IEnumerator DelayRotation()
    {
        float delay = Random.Range(2.5f, 4f);
        yield return new WaitForSeconds(delay);
        yield return Rotation();
    }

    private IEnumerator Rotation()
    {
        _isKnocked = true;
        float elapsedTime = 0f;
        float rotationDuration = 2f;
        choice = Random.Range(0, degree.Length);
        Quaternion startRotation = transform.parent.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, degree[choice], 0f);

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            transform.parent.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.parent.localRotation = targetRotation;
        _isKnocked = false;
    }

>>>>>>> Stashed changes
    private IEnumerator Knocked()
    /*Set player state to knocked and add given force towards spawn direction (invert of moving direction), then wait x seconds and disable knowked state*/
    {
        _isKnocked = true;
<<<<<<< Updated upstream

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

=======
        moveSpeed *= 2f;
        yield return new WaitForSeconds(pushBackTime);
        moveSpeed *= 0.5f;
        _isKnocked = false;

    }
>>>>>>> Stashed changes
}

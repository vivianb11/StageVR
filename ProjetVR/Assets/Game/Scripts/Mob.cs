using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Mob : MonoBehaviour
{
    [Header("Mob Characteristics")]
    [ReadOnly] public Transform target;
    [SerializeField] int lifepoints;
    [SerializeField] float moveSpeed;
    [SerializeField] int scoreAddedOnKill;
    [SerializeField] bool isKnockable;
    public bool canRotate;

    [Header("On Hit Parameters")]
    [ShowIf("isKnockable")] [SerializeField] float knockCooldown;
    [ShowIf("isKnockable")] [SerializeField] int receivedDamagedOnHit;

    [Header("Rotation Parameters")]
    [ShowIf("canRotate")] public int[] degree;

    private bool _isKnocked = false;

    private void Start()
    {
        if (! canRotate) return;
        
        GetComponent<BoxCollider>().isTrigger = true;
        StartCoroutine(DelayRotation());
    }

    private void Update()
    {
        Move();
        IsDeadCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shield")) Damaged();
        if (other.gameObject.CompareTag("Protected")) Attack(other.gameObject);
    }

    private IEnumerator DelayRotation()
    {
        float timeBeforeRotation = Random.Range(2.5f, 4f);
        yield return new WaitForSeconds(timeBeforeRotation);
        yield return Rotation();
    }

    private IEnumerator Rotation()
    {
        _isKnocked = true;
        float elapsedTime = 0f;
        float rotationDuration = 2f;
        int RandomRotationIndex = Random.Range(0, degree.Length);
        Quaternion startRotation = transform.parent.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, degree[RandomRotationIndex], 0f);

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

    private IEnumerator Knocked()
    {
        _isKnocked = true;
        moveSpeed *= 2f;
        yield return new WaitForSeconds(knockCooldown);
        moveSpeed *= 0.5f;
        _isKnocked = false;
    }

    private void Move()
    {
        transform.LookAt(target.position);

        if (_isKnocked) 
        {
           if (isKnockable) transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            return;
        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void Damaged()
    {
        lifepoints -= receivedDamagedOnHit;
        if (isKnockable) StartCoroutine(Knocked());
    }

    private void Attack(GameObject protectedTooth)
    {
        protectedTooth.GetComponent<ProtectedToothBehaviours>().Damaged();
        Kill();
    }

    private bool IsDeadCheck()
    {
        bool condition = lifepoints == 0;
        if (condition) Kill();
        return condition;
    }

    private void Kill()
    {
        ScoreManager.Instance.AddScore(scoreAddedOnKill);
        if (transform.parent) Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}

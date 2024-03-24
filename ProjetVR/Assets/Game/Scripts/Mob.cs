using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Mob : MonoBehaviour
{
    [Header("Mob Characteristics")]
    [ReadOnly] public Transform target;
    [SerializeField] int lifepoints;
    [SerializeField] float moveSpeed;
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

        //transform.parent.localRotation = targetRotation;
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
        transform.LookAt(target.position, transform.parent.up);

        if (_isKnocked) 
        {
           if (isKnockable) transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);
            return;
        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
    }

    private void Damaged()
    {
        lifepoints -= receivedDamagedOnHit;
        IsDeadCheck();

        if (isKnockable) StartCoroutine(Knocked());
    }

    private void Attack(GameObject protectedTooth)
    {
        protectedTooth.GetComponent<ProtectedToothBehaviours>().Damaged();
    }

    private bool IsDeadCheck()
    {
        bool condition = lifepoints == 0;
        if (condition)
        {
            Destroy(gameObject);
        }
        return condition;
    }
}

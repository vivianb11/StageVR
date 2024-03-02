using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectilSpeed;
    public int damage;

    public float randomness = 0.5f;

    public float lifeTime;

    private Rigidbody body;

    private bool canCollid = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Vector3 launchDirection = transform.forward + Vector3.right * Random.Range(-randomness, randomness) + Vector3.up * Random.Range(-randomness, randomness);

        body.AddForce(launchDirection * projectilSpeed);

        StartCoroutine(KillTimer());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canCollid)
            return;
        
        if (other.gameObject.tag != "Cell")
        {
            Destroy(gameObject);
            return;
        }

        if (other.TryGetComponent(out CellBehavior cell) && cell.teethState == TeethState.Tartar && !cell.ToothPasteFull())
        {
            body.velocity = Vector3.zero;
            GetComponent<Collider>().enabled = false;

            StopAllCoroutines();
            body.isKinematic = true;
            transform.parent = other.transform;

            cell.IncreaseToothPasteAmount();

            canCollid = true;

            return;
        }

        Destroy(gameObject);
    }

    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}

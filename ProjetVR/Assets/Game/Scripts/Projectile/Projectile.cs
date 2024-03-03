using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float projectilSpeed;
    public float randomness = 0.5f;
    public float lifeTime;

    protected Rigidbody body;

    protected bool canCollid = true;

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

    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}

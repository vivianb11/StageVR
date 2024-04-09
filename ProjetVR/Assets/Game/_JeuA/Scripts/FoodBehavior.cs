using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodBehavior : MonoBehaviour
{
    private Rigidbody rb;

    public float force = 500f;

    private bool ejected = false;

    [SerializeField] GameObject Explosion;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!ejected)
            return;

        transform.rotation = Quaternion.LookRotation(rb.velocity);

        transform.GetChild(0).transform.Rotate(0, 0, 500 * Time.deltaTime);
    }


    [Button]
    public void EjectFood()
    {
        ejected = true;

        rb.useGravity = true;

        rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0.2f, 1f), Random.Range(-1f, 1f)) * force);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (rb.velocity != Vector3.zero && ejected)
        {
            StartCoroutine(DestroyFood());
        }
    }

    private IEnumerator DestroyFood()
    {
        Explosion.SetActive(true);

        gameObject.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}

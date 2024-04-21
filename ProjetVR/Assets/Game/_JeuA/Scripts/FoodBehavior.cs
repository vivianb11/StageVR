using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInstance player;

    public float delayBeforeDestroayable = 1f;
    private bool isDestroyable = false;

    public float force = 300f;

    private bool ejected = false;

    [SerializeField] GameObject Explosion;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = PlayerInstance.Instance;
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
        transform.parent = null;
        ejected = true;

        rb.isKinematic = false;
        rb.useGravity = true;

        Transform playerpos = player.transform.GetChild(0).transform;
        Vector3 direction = playerpos.up * Random.Range(0.5f, 1f) + Random.Range(-0.5f, 0.5f) * playerpos.right;

        rb.AddForce(direction * force);

        StartCoroutine(DestroyableDelay());
    }

    private void OnCollisionStay(Collision collision)
    {

        if (rb.velocity != Vector3.zero && ejected && isDestroyable)
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

    private IEnumerator DestroyableDelay()
    {
        yield return new WaitForSeconds(delayBeforeDestroayable);

        isDestroyable = true;
    }
}

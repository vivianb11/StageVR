using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodBehavior : MonoBehaviour
{
    private Rigidbody rb;
    public Tween tween;

    public float force = 10f;

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
    }


    [Button]
    public void EjectFood()
    {
        ejected = true;

        rb.useGravity = true;

        rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)) * force);

        StartCoroutine(DestroyFood());
    }

    private IEnumerator DestroyFood()
    {
        TweenMontage tm; 
        
        tween.PlayTween("Spin", out tm);

        yield return new WaitForSeconds(tween.GetMontageDuration(tm));

        Explosion.SetActive(true);

        gameObject.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}

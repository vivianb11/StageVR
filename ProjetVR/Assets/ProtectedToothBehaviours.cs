using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ProtectedToothBehaviours : MonoBehaviour
{
    [Header("Tooth Characteristics")]
    [SerializeField] int health;
    [SerializeField] int receivedDamagedOnHit;
    [SerializeField] UnityEvent onDamaged = new UnityEvent();
    [SerializeField] UnityEvent onDeath = new UnityEvent();
    [SerializeField] Material material2HP; //the material the tooth has at 2 hp
    [SerializeField] Material material1HP; //the material the tooth has at 1 hp

    [SerializeField] float shakeMagnitude = 0.1f;
    [SerializeField] float shakeSpeed = 50f;
    [SerializeField] float shakeDuration = 3f;

    private Vector3 originalPosition;

    public GameObject toothExplosion;


    void Start()
    {
        originalPosition = transform.position;
    }

    public void Damaged()
    {
        if (health <= 0) return;
        onDamaged.Invoke();
        health -= receivedDamagedOnHit;
        materialChange();
        IsDeadCheck();
    }


    private void materialChange()
    {
        if (health == 2)
        {
            gameObject.GetComponent<Renderer>().material = material2HP;
        }

        if (health == 1)
        {
            gameObject.GetComponent<Renderer>().material = material1HP;
        }
    }

    IEnumerator ShakeAndDie()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.position = originalPosition + shakeOffset;
            elapsedTime += Time.deltaTime * shakeSpeed;
            yield return null;
        }

        Instantiate(toothExplosion); //the tooth explodes into many pieces
        Destroy(gameObject);

        yield return null;
    }

    private bool IsDeadCheck()
    {
        bool condition = health == 0;
        if (condition)
        {
            onDeath.Invoke();
            StartCoroutine(ShakeAndDie());
        }
        return condition;
    }

    
}

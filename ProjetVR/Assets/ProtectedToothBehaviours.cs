using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private OutlineScale _outlineScaleEffect;
    private Outline _outlineEffect;
    private float _originalWidth = 5f;
    private float _finalWidth = 10f;
    private float _currentWidth;
    private float _speed = 0.5f;



    void Start()
    {
        originalPosition = transform.position;
        _outlineScaleEffect = GetComponent<OutlineScale>();
        _outlineEffect = GetComponent<Outline>();
        _outlineEffect.enabled = false;
    }

    public void Damaged()
    {
        if (health <= 0) return;
        onDamaged.Invoke();
        health -= receivedDamagedOnHit;
        materialChange();
        //ScaleInStep();
        IsDeadCheck();

    }

    
    // private void Update()
    // {
    //     _currentWidth += _speed * Time.deltaTime;
    //     _currentWidth = Mathf.Clamp(_currentWidth, _originalWidth, _finalWidth);
    //     float pingPongValue = Mathf.PingPong(_currentWidth, _finalWidth - _originalWidth) + _originalWidth;
    //     _outlineEffect.OutlineWidth = pingPongValue;
    // }

    // IEnumerator PingPong()
    // {
    //     float currentValue = _originalWidth;
    //     bool increasing = true;

    //     while (true)
    //     {
    //         // Increment or decrement currentValue based on time and speed
    //         if (increasing)
    //             currentValue += _speed * Time.deltaTime;
    //         else
    //             currentValue -= _speed * Time.deltaTime;

    //         // Check if currentValue reaches the boundaries, and change direction accordingly
    //         if (currentValue >= _finalWidth)
    //             increasing = false;
    //         else if (currentValue <= _finalWidth)
    //             increasing = true;

    //         // Clamp currentValue between minValue and maxValue
    //         currentValue = Mathf.Clamp(currentValue, _originalWidth, _finalWidth);

    //         // Now you can use currentValue for whatever purpose you need, for example, changing the position of an object along an axis.
    //         float pingPongValue = currentValue;
    //         // Now you can use pingPongValue for whatever purpose you need, for example, changing the position of an object along an axis.
    //         _outlineEffect.OutlineWidth = pingPongValue;

    //         yield return null;
    //     }
    // }


    // private void ScaleInStep()
    // {
    //     if (health != 1) return; 
    //     _outlineScaleEffect.ScaleIn();
    //     Invoke("ScaleOutStep", 1f);
    // }

    // private void ScaleOutStep()
    // {
    //     if (health != 1) return; 
    //     _outlineScaleEffect.ScaleOut();
    //     Invoke("ScaleInStep", 1f);
    // }


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

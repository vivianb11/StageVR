using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class BumperCar : MonoBehaviour
{
    public GameObject targetObject;
    public float moveSpeed = 5f;
    public float returnSpeed = 2f;

    private Vector3 originalPosition;
    public bool isColliding = false;
    public float moveDuration = 0.5f;
    private float timer = 0.0f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isColliding)
        {
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, targetObject.transform.position);
            float moveDistance = Mathf.Min(returnSpeed * Time.deltaTime, distance);
            transform.position += direction * moveDistance;

            timer += Time.deltaTime;
            if (timer >= moveDuration)
            {
                isColliding = false;
                timer = 0.0f;
            }
        }
        else
        {
            // Return to original position
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Mob1(Clone)")
        {
            isColliding = true;
        }

        if (other.gameObject.name == "Mob2(Clone)")
        {
            isColliding = true;
        }

        if (other.gameObject.name == "Mob3(Clone)")
        {
            isColliding = true;
        }
    }
}

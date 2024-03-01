using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public Transform target; 

    public float moveSpeed = 5f;

    void Update()
    {
        transform.LookAt(target.position);

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}

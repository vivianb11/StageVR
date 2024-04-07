using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatTowadsVelocity : MonoBehaviour
{
    public Rigidbody rb;

    private void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            // make the up face the direction of the velocity
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}

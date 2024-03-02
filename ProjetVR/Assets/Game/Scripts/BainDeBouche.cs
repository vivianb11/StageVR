using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BainDeBouche : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float shootDelay;

    [SerializeField] GameObject projectile;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 0f, shootDelay);
    }

    void Update()
    {
        transform.position = EyeManager.Instance.hitPosition + Vector3.up * distance;
    }

    private void Shoot()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);
    }
}

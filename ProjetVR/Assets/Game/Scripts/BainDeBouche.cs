using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BainDeBouche : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float shootDelay;

    [SerializeField] GameObject projectile;

    private bool followHitPosition;

    private float currentShootDelay;

    void Update()
    {
        if (followHitPosition)
        {
            transform.position = EyeManager.Instance.hitPosition + Vector3.up * distance;
            ShootProjectile();
        }
    }

    public void EnableShoot()
    {
        followHitPosition = true;
    }

    public void DisableShoot()
    {
        followHitPosition = false;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void ShootProjectile()
    {
        currentShootDelay += Time.deltaTime;

        if (currentShootDelay >= shootDelay)
        {
            currentShootDelay = 0f;

            Instantiate(projectile, transform.position, Quaternion.identity);
        }
    }
}

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    private bool canShoot = false;

    public Slider slider;

    public float blasterShootSpeed;

    [Header("Projectil")]
    public GameObject projectile;
    public float projectileShootSpeed = 0.25f;
    private float currentProjectileShootSpeed;

    private void FixedUpdate()
    {
        if (!canShoot)
            return;

        ShootProjectile();
    }

    [Button]
    public void EnableShoot()
    {
        canShoot = true;
    }

    [Button]
    public void DisableShoot()
    { 
        canShoot = false;
    }

    private void ShootProjectile()
    {
        currentProjectileShootSpeed += Time.deltaTime;

        if (currentProjectileShootSpeed >= projectileShootSpeed)
        {
            currentProjectileShootSpeed = 0f;

            ObjectPooling.Instance.InstantiateGameObject(projectile, transform.position, Quaternion.identity).transform.LookAt(EyeManager.Instance.hitPosition);
        }
    }
}

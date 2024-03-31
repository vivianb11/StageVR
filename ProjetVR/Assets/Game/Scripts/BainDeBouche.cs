using UnityEngine;

public class BainDeBouche : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float shootDelay;

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject particles;

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

        particles.SetActive(true);
    }

    public void DisableShoot()
    {
        followHitPosition = false;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 90f, 90f);

        particles.SetActive(false);
    }

    private void ShootProjectile()
    {
        currentShootDelay += Time.deltaTime;

        if (currentShootDelay >= shootDelay)
        {
            currentShootDelay = 0f;

            ObjectPooling.Pooling.InstantiateGameObject(projectile, transform.position, Quaternion.identity);
        }
    }
}

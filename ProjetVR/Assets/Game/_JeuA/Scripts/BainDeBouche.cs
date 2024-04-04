using UnityEngine;

public class BainDeBouche : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float shootDelay;

    [SerializeField] GameObject projectile;
    [SerializeField] ParticleSystem particles;

    private bool followHitPosition;

    private float currentShootDelay;

    void Update()
    {
        if (followHitPosition)
        {
            transform.position = EyeManager.Instance.hitPosition + Vector3.up * distance;

            if (EyeManager.Instance.RaycastForward(out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Cell"))
                {
                    ShootProjectile();
                    particles.Play();
                }
                else
                    particles.Stop();
            }
            else
                particles.Stop();
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
        transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
    }

    private void ShootProjectile()
    {
        currentShootDelay += Time.deltaTime;

        if (currentShootDelay >= shootDelay)
        {
            currentShootDelay = 0f;

            ObjectPooling.Instance.InstantiateGameObject(projectile, transform.position, Quaternion.identity);
        }
    }
}

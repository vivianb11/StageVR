using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public enum ShootType
    {
        LIQUID, LASER, BLASTER
    }

    public ShootType shootType = ShootType.BLASTER;

    private bool canShoot = false;

    public Slider slider;

    public float blasterShootSpeed;

    [Header("Blaster/Laser")]
    public LaserBeam leftLaser;
    public LaserBeam rightLaser;

    [Header("Projectil")]
    public GameObject projectile;
    public float projectileShootSpeed = 0.25f;
    private float currentProjectileShootSpeed;

    private void Start()
    {
        EyeManager.Instance.stateChanged.AddListener(OnManagerStateChanged);
    }

    private void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.F))
        {
            shootType++;
            if (((int)shootType) > 2)
                shootType = 0;

            SwitchShootType(shootType);
        }
    }

    private void FixedUpdate()
    {
        if (!canShoot)
            return;

        switch (shootType)
        {
            case ShootType.LIQUID:
                ShootProjectile();
                break;
            case ShootType.LASER:
                ShootLaser();
                break;
            case ShootType.BLASTER:
                ShootBlaster();
                break;
        }
    }

    private void OnManagerStateChanged(EyeManager.ManagerState state)
    {
        canShoot = state == EyeManager.ManagerState.SHOOT;
    }

    private void SwitchShootType(ShootType newShootType)
    {
        shootType = newShootType;

        leftLaser.DisableLaser();
        rightLaser.DisableLaser();

        switch (shootType)
        {
            case ShootType.LIQUID:
                break;
            case ShootType.LASER:
                leftLaser.EnableLaser();
                rightLaser.EnableLaser();
                break;
            case ShootType.BLASTER:
                break;
        }
    }

    private void ShootProjectile()
    {
        currentProjectileShootSpeed += Time.deltaTime;

        if (currentProjectileShootSpeed >= projectileShootSpeed)
        {
            currentProjectileShootSpeed = 0f;

            Instantiate(projectile, transform.position, Quaternion.identity).transform.LookAt(transform.position + EyeManager.Instance.GetCursorForward());
        }
    }

    private void ShootLaser()
    {
        if (EyeManager.Instance.RaycastForward(out RaycastHit hit))
        {

        }
    }

    private void ShootBlaster()
    {
        if (!EyeManager.Instance.RaycastForward(out RaycastHit hit))
            return;

        if (!hit.collider.TryGetComponent(out IDamageable damageable))
            return;

        slider.maxValue = blasterShootSpeed * 4;
        slider.value += Time.deltaTime;

        if (slider.value >= slider.maxValue / 4)
        {
            leftLaser.Shoot();
            rightLaser.Shoot();

            damageable.Kill();
            slider.value = 0;
        }
    }
}

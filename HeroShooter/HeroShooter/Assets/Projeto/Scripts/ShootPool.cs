using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class ShootPool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int poolSize = 20;
    public int currentAmmo;
    private int activeProjectiles = 0;
    private ObjectPool<GameObject> pool;
    public bool isEnemy = false;
    private bool canShoot = false;
    private float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public LayerMask layerMask;
    public Transform player;
    public GameManager gameManager;

    void Awake()
    {
        currentAmmo = poolSize;

        pool = new ObjectPool<GameObject>(
            () => Instantiate(projectilePrefab),
            projectile => projectile.SetActive(true),
            projectile => projectile.SetActive(false),
            projectile => Destroy(projectile),
            false,
            poolSize,
            poolSize
        );
    }
    void Update()
    {
        // Impede tiros e recargas antes do início da partida
        if (gameManager == null || !gameManager.GameStarted)
            return;

        if (isEnemy && canShoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (isEnemy && currentAmmo <= 0 && !IsInvoking(nameof(Reload)))
        {
            Invoke(nameof(Reload), 2f);
        }

        if (!isEnemy &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }

        if (!isEnemy &&
            Keyboard.current != null &&
            Keyboard.current[Key.R].isPressed &&
            !IsInvoking(nameof(Reload)))
        {
            Invoke(nameof(Reload), 2f);
        }
    }
    void FixedUpdate()
    {
        if (gameManager == null || !gameManager.GameStarted)
        {
            canShoot = false;
            return;
        }

        canShoot = false;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out RaycastHit hit,
            7f,
            layerMask))
        {
            canShoot = true;
            Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            Debug.DrawLine(
                transform.position,
                transform.position + transform.forward * 100f,
                Color.red
            );
        }
    }
    void Shoot()
    {
        if (currentAmmo <= 0 || activeProjectiles >= poolSize)
            return;

        GameObject projectile = pool.Get();

        currentAmmo++;
        currentAmmo -= 2;
        activeProjectiles++;

        projectile.transform.SetPositionAndRotation(
            firePoint.position,
            firePoint.rotation
        );

        projectile.GetComponent<Projectile>()
            .StartProjectile(firePoint.forward, this);
    }
    public void ReturnProjectile(GameObject projectile)
    {
        if (activeProjectiles > 0)
            activeProjectiles--;

        pool.Release(projectile);
    }

    private void Reload()
    {
        currentAmmo = poolSize;
    }
}
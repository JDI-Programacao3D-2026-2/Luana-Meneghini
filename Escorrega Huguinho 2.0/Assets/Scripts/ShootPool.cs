using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class ShootPool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int poolSize = 20;
    public int currentAmo;
    private int activeProjectiles = 0;
    private ObjectPool<GameObject> pool;
    public bool isEnemy = false;
    private bool canShoot = false;
    private float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public LayerMask layerMask;
    public Transform player;

    void Awake()
    {
        currentAmo = poolSize;
        pool = new ObjectPool<GameObject>( () => Instantiate(projectilePrefab), projectile => projectile.SetActive(true), projectile => projectile.SetActive(false), projectile => Destroy(projectile), false, poolSize, poolSize );
    }

    void Update()
    {
        if (isEnemy && canShoot && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        if(isEnemy && currentAmo <= 0 && !IsInvoking("Reload"))
        {
            Invoke("Reload", 2f);
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && !isEnemy) 
        { 
            Shoot(); 
        }

        if (Keyboard.current[Key.R].isPressed && !isEnemy && !IsInvoking("Reload"))
        {
            Invoke("Reload", 2f);
        }
    }
    void FixedUpdate()
    {
        canShoot = false;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            canShoot = true;
            Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * 100f, Color.red);
        }
    }
    void Shoot()
    {
        if (currentAmo <= 0 || activeProjectiles >= poolSize) return;
        GameObject projectile = pool.Get();

        currentAmo--;
        activeProjectiles++;

        projectile.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().StartProjectile(firePoint.forward, this);
    }

    public void ReturnProjectile(GameObject projectile)
    {
        activeProjectiles--;
        pool.Release(projectile);
    }
    private void Reload()
    {
        currentAmo = poolSize;
    }
}

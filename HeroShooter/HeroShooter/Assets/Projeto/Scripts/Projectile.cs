using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 25f;

    private Vector3 direction;
    private ShootPool shootPool;

    void OnEnable()
    {
        CancelInvoke(nameof(SpawnTime));
        Invoke(nameof(SpawnTime), 5f);
    }

    public void StartProjectile(Vector3 direction, ShootPool shooter)
    {
        this.direction = direction;
        this.shootPool = shooter;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        else if (player != null)
        {
            player.TakeDamage(damage);
        }

        CancelInvoke(nameof(SpawnTime));

        if (shootPool != null)
        {
            shootPool.ReturnProjectile(gameObject);
        }
    }

    void SpawnTime()
    {
        if (shootPool != null)
        {
            shootPool.ReturnProjectile(gameObject);
        }
    }
}

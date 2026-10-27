
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
        if (shootPool == null)
            return;

        EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
        PlayerHealth player = collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (shootPool.isEnemy)
        {
            // Bala inimiga: só causa dano ao jogador.
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        else
        {
            // Bala do jogador: só causa dano aos inimigos.
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        CancelInvoke(nameof(SpawnTime));
        shootPool.ReturnProjectile(gameObject);
    }

    void SpawnTime()
    {
        if (shootPool != null)
        {
            shootPool.ReturnProjectile(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

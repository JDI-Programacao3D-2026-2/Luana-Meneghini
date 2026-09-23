using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private ShootPool ShootPool;

    public void StartProjectile(Vector3 direction, ShootPool shooter)
    {
        this.direction = direction;
        this.ShootPool = shooter;
    }
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime; 
    }
    void OnCollisionEnter(Collision collision)
    {
        ShootPool.ReturnProjectile(gameObject);
    }
}

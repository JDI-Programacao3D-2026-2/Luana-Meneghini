using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float throwForce = 12f;
    public float explosionDelay = 2f;
    public float explosionRadius = 5f;
    public float damage = 60f;
    private Rigidbody rb;
    private bool hasExploded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.AddForce(direction * throwForce, ForceMode.Impulse);

        StartCoroutine(ExplodeAfterDelay());
    }
    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }
    void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        HashSet<EnemyHealth> damagedEnemies =
            new HashSet<EnemyHealth>();

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy =
                hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null && damagedEnemies.Add(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        Debug.Log("Granada explodiu!");

        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
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

        // Libera a física e aplica uma força de impulso
        rb.isKinematic = false;
        rb.AddForce(direction * throwForce, ForceMode.Impulse);

        // Inicia a contagem para a explosão
        StartCoroutine(ExplodeAfterDelay());
    }
    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }
    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Cria uma esfera de colisão invisível na posição para detectar objetos próximos
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        // Utiliza HashSet para garantir que o mesmo inimigo não receba dano duplicado se tiver múltiplos colliders
        HashSet<EnemyHealth> damagedEnemies = new HashSet<EnemyHealth>();

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            // Adiciona ao HashSet e, se for a primeira vez que é inserido, aplica o dano
            if (enemy != null && damagedEnemies.Add(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        Debug.Log("Granada explodiu!");

        Destroy(gameObject);
    }
    // Desenha uma esfera vermelha na janela Scene da Unity para visualizar o alcance no editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
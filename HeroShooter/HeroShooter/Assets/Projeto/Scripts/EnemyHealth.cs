using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;
    public CharacterStats PlayerStats;
    public CharacterStats myStats;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        if (isDead) return;

        isDead = true;

        PlayerStats.AddKill();
        myStats.AddDeath();

        // Avisa o GameManager que um inimigo foi derrotado
        PlayerStats.gameManager.EnemyDefeated();

        gameObject.SetActive(false);
    }

    private Coroutine revealCoroutine;
    private Renderer[] enemyRenderers;
    private Color[][] originalColors;

    public void Reveal(float duration)
    {
        if (revealCoroutine != null)
            StopCoroutine(revealCoroutine);

        if (enemyRenderers == null)
        {
            enemyRenderers = GetComponentsInChildren<Renderer>();

            originalColors = new Color[enemyRenderers.Length][];

            for (int i = 0; i < enemyRenderers.Length; i++)
            {
                Material[] materials = enemyRenderers[i].materials;
                originalColors[i] = new Color[materials.Length];

                for (int j = 0; j < materials.Length; j++)
                    originalColors[i][j] = materials[j].color;
            }
        }

        revealCoroutine = StartCoroutine(RevealEffect(duration));
    }
    private System.Collections.IEnumerator RevealEffect(float duration)
    {
        foreach (Renderer renderer in enemyRenderers)
        {
            foreach (Material material in renderer.materials)
                material.color = Color.yellow;
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < enemyRenderers.Length; i++)
        {
            if (enemyRenderers[i] == null)
                continue;

            Material[] materials = enemyRenderers[i].materials;

            for (int j = 0; j < materials.Length; j++)
            {
                if (j < originalColors[i].Length)
                    materials[j].color = originalColors[i][j];
            }
        }

        revealCoroutine = null;
    }
}
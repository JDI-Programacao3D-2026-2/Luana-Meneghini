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

        // Atualiza a contagem de kills do jogador e mortes do inimigo
        PlayerStats.AddKill();
        myStats.AddDeath();

        // Avisa o GameManager que um inimigo foi derrotado
        PlayerStats.gameManager.EnemyDefeated();

        // Desativa o GameObject do inimigo
        gameObject.SetActive(false);
    }
    
    // Variáveis para gerenciar o efeito visual de revelação por habilidade
    private Coroutine revealCoroutine;
    private Renderer[] enemyRenderers;
    private Color[][] originalColors;

    public void Reveal(float duration)
    {
        // Se já houver um efeito ativo, interrompe a rotina anterior
        if (revealCoroutine != null)
            StopCoroutine(revealCoroutine);

        // Mapeia e guarda os materiais e cores originais do modelo 3D no primeiro uso
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

        // Inicia a Corrotina que alterará a cor
        revealCoroutine = StartCoroutine(RevealEffect(duration));
    }
    private System.Collections.IEnumerator RevealEffect(float duration)
    {
        // Troca a cor de todos os materiais para Amarelo
        foreach (Renderer renderer in enemyRenderers)
        {
            foreach (Material material in renderer.materials)
                material.color = Color.yellow;
        }

        // Espera o tempo definido
        yield return new WaitForSeconds(duration);

        // Restaura as cores originais dos materiais
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
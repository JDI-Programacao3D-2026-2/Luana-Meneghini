using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;
    public CharacterStats myStats;
    public bool IsShielded { get; private set; } = false;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        // Se a habilidade de Escudo do Defensor estiver ativa, bloqueia todo o dano
        if (IsShielded)
        {
            Debug.Log("Escudo bloqueou o dano!");
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log("Vida do player: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        if (isDead) return;

        isDead = true;

        myStats.AddDeath(); // Registra a morte do jogador no placar

        // Reinicia o nível após 2 segundos
        Invoke(nameof(RestartLevel), 2f);
    }
    void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
    public bool Heal(float amount)
    {
        // Não cura se estiver morto ou com a vida cheia
        if (isDead || currentHealth >= maxHealth)
            return false;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        Debug.Log("Vida recuperada: " + currentHealth);

        return true;
    }
    public void SetShield(bool active)
    {
        IsShielded = active;
    }
}
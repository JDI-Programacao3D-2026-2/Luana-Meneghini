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
}
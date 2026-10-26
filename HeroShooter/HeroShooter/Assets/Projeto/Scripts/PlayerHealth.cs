using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    public CharacterStats myStats;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

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

        myStats.AddDeath();

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
        if (isDead || currentHealth >= maxHealth)
            return false;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        Debug.Log("Vida recuperada: " + currentHealth);

        return true;
    }
}




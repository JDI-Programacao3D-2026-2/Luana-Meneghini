using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public CharacterStats PlayerStats;
    public CharacterStats myStats;

    void Start(){
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount){
        currentHealth -= amount;
        if (currentHealth <= 0f)  {
            Die();
        }
    }
    void Die(){
        PlayerStats.AddKill();
        myStats.AddDeath();
        gameObject.SetActive(false);
    }
}


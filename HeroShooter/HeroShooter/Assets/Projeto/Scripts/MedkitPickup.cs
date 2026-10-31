using UnityEngine;
using UnityEngine.InputSystem;

public class MedkitPickup : MonoBehaviour
{
    public float healAmount = 40f;
    private PlayerHealth nearbyPlayer;
    private bool used = false;

    void Update()
    {
        if (used || nearbyPlayer == null)
            return;

        // Se o jogador pressionar 'Q' enquanto estiver na área
        if (Keyboard.current != null &&
            Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            // Tenta curar o jogador; se der certo, consome o kit de cura
            if (nearbyPlayer.Heal(healAmount))
            {
                used = true;
                gameObject.SetActive(false);
                Debug.Log("Kit de cura utilizado!");
            }
            else
            {
                Debug.Log("Sua vida já está cheia!");
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        CharacterStats stats =
            other.GetComponentInParent<CharacterStats>();

        PlayerHealth health =
            other.GetComponentInParent<PlayerHealth>();

        // Registra o jogador ao entrar na área
        if (stats != null &&
            stats.team == CharacterStats.TeamType.Player &&
            health != null)
        {
            nearbyPlayer = health;
        }
    }
    void OnTriggerExit(Collider other)
    {
        PlayerHealth health =
            other.GetComponentInParent<PlayerHealth>();

        // Limpa a referência ao sair da área
        if (health != null && health == nearbyPlayer)
        {
            nearbyPlayer = null;
        }
    }
}
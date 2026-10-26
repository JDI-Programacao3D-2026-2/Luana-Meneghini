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

        if (Keyboard.current != null &&
            Keyboard.current[Key.Q].wasPressedThisFrame)
        {
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

        if (health != null && health == nearbyPlayer)
        {
            nearbyPlayer = null;
        }
    }
}
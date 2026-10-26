using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponCrate : MonoBehaviour
{
    private Skills nearbyPlayer;
    private bool used = false;

    void Update()
    {
        if (used || nearbyPlayer == null)
            return;

        if (Keyboard.current != null &&
            Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            nearbyPlayer.EquipGrenade();

            used = true;
            gameObject.SetActive(false);

            Debug.Log("Granada equipada!");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        CharacterStats stats =
            other.GetComponentInParent<CharacterStats>();

        Skills skills =
            other.GetComponentInParent<Skills>();

        if (stats != null &&
            stats.team == CharacterStats.TeamType.Player &&
            skills != null)
        {
            nearbyPlayer = skills;
        }
    }
    void OnTriggerExit(Collider other)
    {
        Skills skills =
            other.GetComponentInParent<Skills>();

        if (skills != null && skills == nearbyPlayer)
        {
            nearbyPlayer = null;
        }
    }
}
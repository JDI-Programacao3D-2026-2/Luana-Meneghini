using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 20;
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        CharacterStats stats =
            other.GetComponentInParent<CharacterStats>();

        ShootPool weapon =
            other.GetComponentInParent<ShootPool>();

        // Somente o jogador pode coletar
        if (stats == null ||
            stats.team != CharacterStats.TeamType.Player)
            return;

        if (weapon == null || weapon.isEnemy)
            return;

        int previousAmmo = weapon.currentAmmo;

        weapon.currentAmmo = Mathf.Min(
            weapon.currentAmmo + ammoAmount,
            weapon.poolSize
        );

        if (weapon.currentAmmo > previousAmmo)
        {
            collected = true;
            gameObject.SetActive(false);

            Debug.Log("Munição recolhida pelo jogador!");
        }
    }
}
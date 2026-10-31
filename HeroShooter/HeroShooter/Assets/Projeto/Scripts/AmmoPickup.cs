using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 20;
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        CharacterStats stats = other.GetComponentInParent<CharacterStats>();
        ShootPool weapon = other.GetComponentInParent<ShootPool>();

        // Somente o jogador pode coletar
        if (stats == null ||
            stats.team != CharacterStats.TeamType.Player)
            return;

        // Garante que o objeto atacante não seja um inimigo
        if (weapon == null || weapon.isEnemy)
            return;

        int previousAmmo = weapon.currentAmmo;

        // Mathf.Min impede que a munição ultrapasse o limite total (poolSize)
        weapon.currentAmmo = Mathf.Min(
            weapon.currentAmmo + ammoAmount,
            weapon.poolSize
        );

        // Se a munição realmente aumentou, consome o objeto
        if (weapon.currentAmmo > previousAmmo)
        {
            collected = true;
            gameObject.SetActive(false);

            Debug.Log("Munição recolhida pelo jogador!");
        }
    }
}
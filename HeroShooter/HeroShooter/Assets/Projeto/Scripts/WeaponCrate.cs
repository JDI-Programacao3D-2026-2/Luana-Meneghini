using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponCrate : MonoBehaviour
{
    private Skills nearbyPlayer; // Armazena a referência do jogador se ele estiver perto
    private bool used = false;

    void Update()
    {
        // Se a caixa já foi usada ou se o jogador não está perto, cancela a verificação
        if (used || nearbyPlayer == null)
            return;

        // Checa se a tecla Q foi pressionada neste frame
        if (Keyboard.current != null &&
            Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            nearbyPlayer.EquipGrenade(); // Executa o método no script do jogador

            used = true;
            gameObject.SetActive(false); // Desativa a caixa da cena

            Debug.Log("Granada equipada!");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // Ao entrar na área de colisão, verifica se é a entidade do Jogador
        CharacterStats stats = other.GetComponentInParent<CharacterStats>();
        Skills skills = other.GetComponentInParent<Skills>();

        if (stats != null &&
            stats.team == CharacterStats.TeamType.Player &&
            skills != null)
        {
            nearbyPlayer = skills; // Salva a referência para permitir a interação no Update
        }
    }
    void OnTriggerExit(Collider other)
    {
        Skills skills = other.GetComponentInParent<Skills>();

        // Ao sair da área, limpa a referência
        if (skills != null && skills == nearbyPlayer)
        {
            nearbyPlayer = null;
        }
    }
}
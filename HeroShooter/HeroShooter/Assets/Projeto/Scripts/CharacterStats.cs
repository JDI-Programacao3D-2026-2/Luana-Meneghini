using System.Security.Cryptography;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public TeamType team = TeamType.Enemy; // Define o time padrão como Inimigo
    public int kills = 0;
    public int deaths = 0;
    public GameManager gameManager; // Referência ao gerenciador da partida

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddPlayerName();
    }
    public void AddKill()
    {
        kills++;
        // Se for o jogador, notifica o GameManager para somar no placar da UI
        if (team == TeamType.Player)
        {
            gameManager.UpdateFragScore(1);
        }
    }
    public void AddDeath()
    {
        deaths++;
        // Se for o jogador, notifica o GameManager para registrar a morte na UI
        if (team == TeamType.Player)
        {
            gameManager.UpdateDeathScore(1);
        }
    }
    public void AddPlayerName()
    {
        if (team == TeamType.Player)
        {
            gameManager.PlayerLists(characterName);
        }
    }
    // Enum para categorizar os tipos de entidades no jogo
    public enum TeamType
    {
        Player, Ally, Enemy
    }
}

using System.Security.Cryptography;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public TeamType team = TeamType.Enemy;
    public int kills = 0;
    public int deaths = 0;
    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddPlayerName();
    }
    public void AddKill()
    {
        kills++;
        if (team == TeamType.Player)
        {
            gameManager.UpdateFragScore(1);
        }
    }
    public void AddDeath()
    {
        deaths++;
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
    public enum TeamType
    {
        Player, Ally, Enemy
    }
}

using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gameStartText;
    public float startTime = 5f;
    public bool StopCountdown;
    public GameObject tabPanel;
    public int FragScore = 0;
    public int DeathScore = 0;
    public TextMeshProUGUI fragText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI playerNameText;
    public int remainingEnemies = 0;
    public bool GameEnded { get; private set; } = false;

    // Indica se a partida já começou
    public bool GameStarted { get; private set; } = false;

    void Start()
    {
        StopCountdown = false;
        GameStarted = false;
        startTime = 5f;

        // Recupera as pontuações anteriores
        FragScore = GameSession.FragScore;
        DeathScore = GameSession.DeathScore;

        fragText.text = FragScore.ToString();
        deathText.text = DeathScore.ToString();

        // Esconde o painel inicialmente
        tabPanel.SetActive(false);

        remainingEnemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None).Length;

        GameEnded = false;
    }
    void Update()
    {
        StartCountDown();
        ScorePanel();
    }
    void StartCountDown()
    {
        if (GameStarted)
            return;

        if (startTime > 0f)
        {
            startTime -= Time.deltaTime;

            gameStartText.text =
                "Game Starts in: " + Mathf.CeilToInt(startTime);
        }
        else
        {
            startTime = 0f;
            GameStarted = true;

            gameStartText.text = "Game Started!";
            Invoke(nameof(ClearCountDown), 1f);
        }
    }
    void ClearCountDown()
    {
        gameStartText.text = "";
    }
    void ScorePanel()
    {
        if (Keyboard.current == null)
            return;

        // Aparece somente enquanto TAB estiver pressionado
        tabPanel.SetActive(Keyboard.current.tabKey.isPressed);
    }
    public void UpdateFragScore(int frag)
    {
        FragScore += frag;
        GameSession.FragScore = FragScore;

        fragText.text = FragScore.ToString();
    }
    public void UpdateDeathScore(int death)
    {
        DeathScore += death;
        GameSession.DeathScore = DeathScore;

        deathText.text = DeathScore.ToString();
    }
    public void PlayerLists(string playerName)
    {
        playerNameText.text = playerName;
    }
    public void EnemyDefeated()
    {
        if (GameEnded) return;

        remainingEnemies--;

        Debug.Log("Inimigos restantes: " + remainingEnemies);

        if (remainingEnemies <= 0)
        {
            EndGame();
        }
    }
    void EndGame()
    {
        GameEnded = true;

        gameStartText.text = "MISSÃO CONCLUÍDA!";
        Debug.Log("MISSÃO CONCLUÍDA!");
    }

}
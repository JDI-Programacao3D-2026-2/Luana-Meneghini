using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] private TMP_Text agentNameText;
    [SerializeField] private TMP_Text abilitiesText;

    private readonly string[] agentNames =
    {
        "Agente 1 - Defensor",
        "Agente 2 - Infiltrador",
        "Agente 3 - Reconhecimento"
    };

    private readonly string[] abilities =
    {
        "Escudo: protege contra dano por 5 segundos.\n\n" +
        "Impacto: empurra os inimigos próximos.",

        "Camuflagem: dificulta a detecção por alguns segundos.\n\n" +
        "Investida: avança rapidamente em uma direção.",

        "Scanner: revela inimigos próximos.\n\n" +
        "Bloqueio de comunicação: impede temporariamente " +
        "que inimigos afetados chamem reforços."
    };

    private void Start()
    {
        SelectAgent(GameSession.SelectedAgent);
    }

    public void SelectAgent(int agentIndex)
    {
        if (agentIndex < 0 || agentIndex >= agentNames.Length)
            return;

        GameSession.SelectedAgent = agentIndex;

        agentNameText.text = agentNames[agentIndex];
        abilitiesText.text = abilities[agentIndex];
    }

    public void ConfirmSelection()
    {
        SceneManager.LoadScene("MapSelect");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}


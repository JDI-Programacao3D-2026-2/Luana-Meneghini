using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    // [SerializeField] expõe os campos privados no Inspetor da Unity para vincular os textos da UI
    [SerializeField] private TMP_Text agentNameText;
    [SerializeField] private TMP_Text abilitiesText;

    // Arrays contendo os nomes e as descrições das habilidades de cada agente
    private readonly string[] agentNames =
    {
        "Agente 1 - Defensor",
        "Agente 2 - Infiltrador",
        "Agente 3 - Reconhecimento"
    };

    private readonly string[] abilities =
    {
        "\n Escudo: protege contra dano por 5 segundos.\n\n" +
        "Impacto: empurra os inimigos próximos.",

        "\n Camuflagem: dificulta a detecção por alguns segundos.\n\n" +
        "Investida: avança rapidamente em uma direção.",

        "Adrenalina: aumenta a velocidade de movimento temporariamente.\n\n" +
        "Bloqueio de comunicação: impede temporariamente " +
        "que inimigos afetados chamem reforços."
    };

    private void Start()
    {
        // Ao iniciar a cena, carrega as informações do agente que já estava selecionado na GameSession
        SelectAgent(GameSession.SelectedAgent);
    }

    public void SelectAgent(int agentIndex)
    {
        // Validação de segurança: previne erros de índice fora do array (IndexOutOfRangeException)
        if (agentIndex < 0 || agentIndex >= agentNames.Length)
            return;

        // Atualiza a escolha na classe estática e atualiza a interface
        GameSession.SelectedAgent = agentIndex;

        agentNameText.text = agentNames[agentIndex];
        abilitiesText.text = abilities[agentIndex];
    }

    public void ConfirmSelection()
    {
        // Transiciona para a cena "MapSelect"
        SceneManager.LoadScene("MapSelect");
    }

    public void GoBack()
    {
        // Retorna ao menu principal
        SceneManager.LoadScene("MainMenu");
    }
}


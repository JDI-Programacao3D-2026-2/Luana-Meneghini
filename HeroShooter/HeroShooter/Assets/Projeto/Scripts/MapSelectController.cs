using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectController : MonoBehaviour
{
    [SerializeField] private Button map2Button;
    [SerializeField] private Button map3Button;

    private void Start()
    {
        // Desativa a interação dos botões de mapas bloqueados
        map2Button.interactable = false;
        map3Button.interactable = false;
    }

    public void PlayMap1()
    {
        SceneManager.LoadScene("Map01");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}


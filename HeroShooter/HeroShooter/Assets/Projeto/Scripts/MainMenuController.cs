
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("O jogo foi fechado");
    }
}


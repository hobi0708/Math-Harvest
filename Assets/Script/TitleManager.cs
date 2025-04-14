using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject levelSelectPanel;

    void Start()
    {
        optionPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
    }

    public void StartGame()
    {
        levelSelectPanel.SetActive(true);
    }

    public void SelectLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SelectLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void SelectLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void BackToTitle()
    {
        levelSelectPanel.SetActive(false);
    }

    public void OpenOption()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
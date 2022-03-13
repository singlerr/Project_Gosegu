using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneHandler : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button exitButton;

    private void Start()
    {
        startButton = GameObject.Find("NewGame").GetComponent<Button>(); // game scene
        loadButton = GameObject.Find("Continue").GetComponent<Button>(); // loading scene
        exitButton = GameObject.Find("Exit").GetComponent<Button>(); // exit game
        exitButton.onClick.AddListener(ExitGame);
        startButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Day1-loading");
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("LoadMenu");
    }
}
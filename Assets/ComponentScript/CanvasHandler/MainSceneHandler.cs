using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneHandler : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button exitButton;
    void Start()
    {
        startButton = GameObject.Find("NewGame").GetComponent<Button>(); // game scene
        loadButton = GameObject.Find("Continue").GetComponent<Button>(); // loading scene
        exitButton = GameObject.Find("Exit").GetComponent<Button>(); // exit game
        exitButton.onClick.AddListener(ExitGame);
        startButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
    }

    void ExitGame()
    {
        Application.Quit();
    }
    void StartGame()
    {
        SceneManager.LoadScene("Day1-loading");
    }
    void LoadGame()
    {
        SceneManager.LoadScene("LoadMenu");
    }
}

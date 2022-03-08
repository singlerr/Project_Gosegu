using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneHandler : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button exitButton;
    private int index = 0;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        startButton = GameObject.Find("NewGame").GetComponent<Button>(); // game scene
        loadButton = GameObject.Find("Continue").GetComponent<Button>(); // loading scene
        exitButton = GameObject.Find("Exit").GetComponent<Button>(); // exit game
        exitButton.onClick.AddListener(Exitgame);
    }

    void Exitgame()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }
}

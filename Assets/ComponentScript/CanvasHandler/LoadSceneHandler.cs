using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneHandler : MonoBehaviour
{
    private int index = 0;
    public string name;
    public string[] savedata;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath+"/SaveData";
        Debug.Log(path);
        savedata = Directory.GetFiles(path, "*.segu");
        foreach (var file in savedata){
            Debug.Log(file);
        }
    }

}

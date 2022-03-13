using System.IO;
using UnityEngine;

public class LoadSceneHandler : MonoBehaviour
{
    public string name;
    public string[] savedata;

    private int index = 0;

    // Start is called before the first frame update
    private void Start()
    {
        var path = Application.dataPath + "/SaveData";
        Debug.Log(path);
        savedata = Directory.GetFiles(path, "*.segu");
        foreach (var file in savedata) Debug.Log(file);
    }
}
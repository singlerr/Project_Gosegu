using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuBehaviour : MonoBehaviour
{
    void Start(){
        Debug.Log("Game start");
    }
    void Update(){
        if(Input.anyKey){
            LoadNextScene();
        }
    }
    private void LoadNextScene(){
        Scene scene = SceneManager.GetActiveScene();
        int nextLevelBuildIndex = 1 - scene.buildIndex;
        SceneManager.LoadScene(nextLevelBuildIndex);
    }
}

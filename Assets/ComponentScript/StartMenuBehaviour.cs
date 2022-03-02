using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 시작메뉴 behavior class
public class StartMenuBehaviour : MonoBehaviour
{
    void Start(){
        Debug.Log("Game start");
    }
    void Update(){
        // on press anykey, loads next screne
        if(Input.anyKey){
            LoadNextScene();
        }
    }
    // Loads next scene in built order.
    private void LoadNextScene(){
        Scene scene = SceneManager.GetActiveScene();
        int nextLevelBuildIndex = 1 - scene.buildIndex;
        SceneManager.LoadScene(nextLevelBuildIndex);
    }
}

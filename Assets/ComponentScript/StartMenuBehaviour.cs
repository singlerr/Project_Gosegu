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
            Debug.Log("Mouse Clicked");
            SceneManager.LoadScene("MainScene");    
        }
    }
    private void OnMouseDown() {
    }
}

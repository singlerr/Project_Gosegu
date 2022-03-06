using UnityEngine;

public class exitButton : MonoBehaviour {
    public void exitGame(){
        Debug.Log("Exit");
        Application.Quit();
    }
}
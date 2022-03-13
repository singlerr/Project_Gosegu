using UnityEngine;
using UnityEngine.SceneManagement;

// 시작메뉴 behavior class
public class StartMenuBehaviour : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Game start");
    }

    private void Update()
    {
        // on press anykey, loads next screne
        if (Input.anyKey) LoadNextScene();
    }

    // Loads next scene in built order.
    private void LoadNextScene()
    {
        var scene = SceneManager.GetActiveScene();
        var nextLevelBuildIndex = 1 - scene.buildIndex;
        SceneManager.LoadScene(nextLevelBuildIndex);
    }
}
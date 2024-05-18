using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName1;
    public string sceneName2;
    public string sceneName3;

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName1);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName2);
    }

    public void LoadInstructionsScene()
    {
        SceneManager.LoadScene(sceneName3);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}

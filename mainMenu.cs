using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//attached to nothing, but buttons use the functions in this script for scene management
public class MainMenu : MonoBehaviour
{
    public void LoadOpenWorld()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

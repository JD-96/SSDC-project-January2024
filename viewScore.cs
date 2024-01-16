using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class viewScore : MonoBehaviour
{
    public static int finalScore;
    public TextMeshProUGUI scoreText; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        scoreText.text = "" + finalScore;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}

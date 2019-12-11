using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    void Update()
    {
        // Restart Game
        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(0);
        // Quit Game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}

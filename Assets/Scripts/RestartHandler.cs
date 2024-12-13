using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHandler : MonoBehaviour
{
    // Name of your main menu scene
    public string mainMenuSceneName = "StartScreen";

    void Update()
    {
        // Check if both left or right shift and R are pressed
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restart triggered - Loading main menu...");
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

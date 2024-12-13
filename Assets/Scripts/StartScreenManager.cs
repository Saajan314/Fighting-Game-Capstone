using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    // Public variables to link buttons in the Inspector
    public Button playButton;
    public Button quitButton;

    void Start()
    {
        // Assign methods to button click events
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CharacterSelection"); // Load the next scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Exit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
#endif
    }
}





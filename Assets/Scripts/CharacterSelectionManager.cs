using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public Button[] playerButtons;
    public Button[] aiButtons;
    public Image playerSelectionDisplay;
    public Image aiSelectionDisplay;
    public Sprite[] characterIcons;  // Ensure this array is large enough for all characters (e.g., 6 for 6 characters)
    public Button confirmButton;

    private int playerChoice = -1;
    private int aiChoice = -1;

    void Start()
    {
        confirmButton.gameObject.SetActive(false); // Hide Confirm button initially

        for (int i = 0; i < playerButtons.Length; i++)
        {
            int index = i;
            playerButtons[i].onClick.AddListener(() => SelectPlayerCharacter(index));
        }

        for (int i = 0; i < aiButtons.Length; i++)
        {
            int index = i;
            aiButtons[i].onClick.AddListener(() => SelectAICharacter(index));
        }

        confirmButton.onClick.AddListener(StartGame);
    }

    public void SelectPlayerCharacter(int index)
    {
        playerChoice = index;
        playerSelectionDisplay.sprite = characterIcons[index];
        CheckSelectionComplete();
    }

    public void SelectAICharacter(int index)
    {
        aiChoice = index;
        aiSelectionDisplay.sprite = characterIcons[index + 3]; // Assuming first 3 are for the player, next 3 are for AI
        CheckSelectionComplete();
    }

    private void CheckSelectionComplete()
    {
        // Show Confirm button only if both characters are selected
        if (playerChoice != -1 && aiChoice != -1)
        {
            confirmButton.gameObject.SetActive(true);
        }
    }

    private void StartGame()
    {
        // Pass selected characters to the Gameplay scene
        PlayerPrefs.SetInt("PlayerChoice", playerChoice);
        PlayerPrefs.SetInt("AIChoice", aiChoice);
        SceneManager.LoadScene("Gameplay");
    }
}




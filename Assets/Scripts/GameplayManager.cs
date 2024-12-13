using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WarriorAnimsFREE;

public class GameplayManager : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform aiSpawnPoint;
    public GameObject[] characterPrefabs; // Array of character prefabs

    public Image playerHealthBar;
    public Image aiHealthBar;

    void Start()
    {
        // Get character choices from PlayerPrefs
        int playerChoice = PlayerPrefs.GetInt("PlayerChoice", -1);
        int aiChoice = PlayerPrefs.GetInt("AIChoice", -1);






        // Ensure valid choices
        if (playerChoice == -1 || aiChoice == -1)
        {
            Debug.LogError("Character choices not set! Returning to Character Selection.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
            return;
        }

        // Spawn Player Character
        GameObject playerCharacter = Instantiate(characterPrefabs[playerChoice], playerSpawnPoint.position, Quaternion.identity);
        playerCharacter.AddComponent<PlayerController>(); // Assign Player Controller

        // Set the player character's tag to "Player"
        playerCharacter.tag = "Player";

        // Spawn AI Character
        GameObject aiCharacter = Instantiate(characterPrefabs[aiChoice], aiSpawnPoint.position, Quaternion.identity);

        aiCharacter.tag = "AI";

        // Remove a specific script (e.g., Remove a script named 'UnwantedScript')
        GUIControls GUIControls = aiCharacter.GetComponent<GUIControls>();
        if (GUIControls != null)
        {
            Destroy(GUIControls);
        }

        WarriorInputController WarriorInputController = aiCharacter.GetComponent<WarriorInputController>();
        if (WarriorInputController != null)
        {
            Destroy(WarriorInputController);
        }


        aiCharacter.AddComponent<AIController>();
        //playerCharacter.AddComponent<Health>();
        //aiCharacter.AddComponent<AIHealth>();
        playerCharacter.AddComponent<CollisionHandler>();
        aiCharacter.AddComponent<CollisionHandler>();



    }
}







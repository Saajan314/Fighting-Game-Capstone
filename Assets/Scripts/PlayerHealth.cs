using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 1000f;  // Default health for the player
    public float damageAmount = 5f;   // Amount of damage taken when colliding with a weapon

    [SerializeField]
    private HealthSystem healthBar;    // Reference to the player's HealthSystem

    void Start()
    {
        // If you haven't assigned the health bar in the inspector, try to find it in the scene.
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthSystem>();  // Find the HealthSystem for the player
        }

        // Set max health for the health bar
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(playerHealth);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            // Apply damage when colliding with an object tagged "Weapon"
            playerHealth -= damageAmount;
            playerHealth = Mathf.Clamp(playerHealth, 0f, Mathf.Infinity);  // Ensure health doesn't go negative

            // Update health bar
            if (healthBar != null)
            {
                healthBar.SetHealth(playerHealth);
            }

            Debug.Log("Player Hit by Weapon! Current Health: " + playerHealth);
        }
    }
}


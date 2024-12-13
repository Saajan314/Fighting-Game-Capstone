using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    public float aiHealth = 1000f;      // Default health for the AI
    public float damageAmount = 5f;   // Amount of damage taken when colliding with a weapon

    [SerializeField]
    private HealthSystem healthBar;    // Reference to the AI's HealthSystem

    void Start()
    {
        // If you haven't assigned the health bar in the inspector, try to find it in the scene.
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthSystem>();  // Find the HealthSystem for the AI
        }

        // Set max health for the health bar
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(aiHealth);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            // Apply damage when colliding with an object tagged "Weapon"
            aiHealth -= damageAmount;
            aiHealth = Mathf.Clamp(aiHealth, 0f, Mathf.Infinity);  // Ensure health doesn't go negative

            // Update health bar
            if (healthBar != null)
            {
                healthBar.SetHealth(aiHealth);
            }

            Debug.Log("AI Hit by Weapon! Current Health: " + aiHealth);
        }
    }
}

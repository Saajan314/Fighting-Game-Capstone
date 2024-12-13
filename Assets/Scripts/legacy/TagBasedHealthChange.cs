using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagBasedHealthChange : MonoBehaviour
{
    public float playerHealth = 100f;  // Default health, can be set in the Inspector
    public float damageAmount = 10f;   // Amount of damage taken when colliding with a weapon

    [SerializeField]
    private HealthSystem healthBar;    // Reference to the HealthSystem to update health bar UI

    void Start()
    {
        // If you haven't assigned the health bar in the inspector, try to find it in the scene.
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthSystem>();
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

            Debug.Log("Hit by weapon! Current Health: " + playerHealth);
        }
    }
}


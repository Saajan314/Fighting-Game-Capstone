using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private HealthManager healthManager;

    void Start()
    {
        healthManager = GameObject.FindObjectOfType<HealthManager>();
        if (healthManager == null)
            Debug.LogError("HealthManager not found in the scene!");
        else
            Debug.Log($"CollisionHandler initialized on {gameObject.name} with tag {gameObject.tag}");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected on {gameObject.name} with {collision.gameObject.name} (Tag: {collision.gameObject.tag})");

        if (collision.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("Weapon collision detected!");

            // PLAYER OR AI TAG CHECK
            if (gameObject.CompareTag("Player"))
            {
                Debug.Log("Player hit by weapon!");
                if (healthManager != null)
                    healthManager.DamagePlayer(healthManager.weaponDamage);
                else
                    Debug.LogError("HealthManager is null when trying to damage player!");
            }
            else if (gameObject.CompareTag("AI"))
            {
                Debug.Log("AI hit by weapon!");
                if (healthManager != null)
                    healthManager.DamageAI(healthManager.weaponDamage);
                else
                    Debug.LogError("HealthManager is null when trying to damage AI!");
            }
        }
    }
 
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger detected on {gameObject.name} with {other.gameObject.name} (Tag: {other.gameObject.tag})");

        if (other.CompareTag("Weapon"))
        {
            Debug.Log("Weapon trigger detected!");

            if (gameObject.CompareTag("Player"))
            {
                Debug.Log("Player hit by weapon trigger!");
                if (healthManager != null)
                    healthManager.DamagePlayer(healthManager.weaponDamage);
                else
                    Debug.LogError("HealthManager is null when trying to damage player!");
            }
            else if (gameObject.CompareTag("AI"))
            {
                Debug.Log("AI hit by weapon trigger!");
                if (healthManager != null)
                    healthManager.DamageAI(healthManager.weaponDamage);
                else
                    Debug.LogError("HealthManager is null when trying to damage AI!");
            }
        }
    }
}

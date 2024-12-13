using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Health, MaxHealth;

    private HealthSystem healthBar;

    void Start()
    {
        // Try to find the HealthSystem component in the scene by tag or name
        healthBar = FindObjectOfType<HealthSystem>();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(MaxHealth);  // Set max health in the health system
        }
        else
        {
            Debug.LogError("HealthSystem not found in the scene!");
        }
    }

    void Update()
    {
        /*        if (Input.GetKeyDown("q"))
                {
                    SetHealth(-20f);
                }
                if (Input.GetKeyDown("e"))
                {
                    SetHealth(20f);
                }*/
    }

    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(Health);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Weapon weapon = other.gameObject.GetComponent<Weapon>();
            if (weapon != null)
            {
                SetHealth(-weapon.GetDamage());
            }
        }
    }
}








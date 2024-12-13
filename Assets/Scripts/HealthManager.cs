using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // Health values
    public float playerMaxHealth = 100f;
    public float aiMaxHealth = 100f;
    private float currentPlayerHealth;
    private float currentAIHealth;

    // Damage values
    public float weaponDamage = 20f;

    // UI References
    public RectTransform playerHealthBar;
    public RectTransform aiHealthBar;

    // Banner Animation References
    public VictoryBannerAnimation victoryBanner;
    public VictoryBannerAnimation defeatBanner;

    // Original width of health bars
    private float originalPlayerBarWidth;
    private float originalAIBarWidth;

    // Track if game is over
    private bool isGameOver = false;

    void Start()
    {
        // Initialize health values
        currentPlayerHealth = playerMaxHealth;
        currentAIHealth = aiMaxHealth;

        // Store original health bar widths
        if (playerHealthBar != null)
            originalPlayerBarWidth = playerHealthBar.sizeDelta.x;
        if (aiHealthBar != null)
            originalAIBarWidth = aiHealthBar.sizeDelta.x;

        Debug.Log($"Health System Initialized - Player Health: {currentPlayerHealth}, AI Health: {currentAIHealth}");

        // Ensure banner animations are initially invisible
        if (victoryBanner != null && victoryBanner.banner != null)
            victoryBanner.banner.gameObject.SetActive(false);
        if (defeatBanner != null && defeatBanner.banner != null)
            defeatBanner.banner.gameObject.SetActive(false);
    }

    // Update health bars
    private void UpdateHealthBars()
    {
        if (playerHealthBar != null)
        {
            float playerHealthPercentage = currentPlayerHealth / playerMaxHealth;
            Vector2 playerSize = playerHealthBar.sizeDelta;
            playerSize.x = originalPlayerBarWidth * playerHealthPercentage;
            playerHealthBar.sizeDelta = playerSize;
        }

        if (aiHealthBar != null)
        {
            float aiHealthPercentage = currentAIHealth / aiMaxHealth;
            Vector2 aiSize = aiHealthBar.sizeDelta;
            aiSize.x = originalAIBarWidth * aiHealthPercentage;
            aiHealthBar.sizeDelta = aiSize;
        }
    }

    // Damage handling methods
    public void DamagePlayer(float damage)
    {
        if (isGameOver) return;

        currentPlayerHealth = Mathf.Max(0, currentPlayerHealth - damage);
        Debug.Log($"Player took {damage} damage. Current health: {currentPlayerHealth}");
        UpdateHealthBars();

        if (currentPlayerHealth <= 0)
        {
            PlayerDeath();
        }
    }

    public void DamageAI(float damage)
    {
        if (isGameOver) return;

        currentAIHealth = Mathf.Max(0, currentAIHealth - damage);
        Debug.Log($"AI took {damage} damage. Current health: {currentAIHealth}");
        UpdateHealthBars();

        if (currentAIHealth <= 0)
        {
            AIDeath();
        }
    }

    private void PlayerDeath()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Player has died!");

        // Show defeat banner
        if (defeatBanner != null && defeatBanner.banner != null)
        {
            defeatBanner.banner.gameObject.SetActive(true);
            StartCoroutine(defeatBanner.AnimateVictoryBanner());
        }
        else
        {
            Debug.LogError("Defeat Banner not assigned in HealthManager!");
        }
    }

    private void AIDeath()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("AI has died!");

        // Show victory banner
        if (victoryBanner != null && victoryBanner.banner != null)
        {
            victoryBanner.banner.gameObject.SetActive(true);
            StartCoroutine(victoryBanner.AnimateVictoryBanner());
        }
        else
        {
            Debug.LogError("Victory Banner not assigned in HealthManager!");
        }
    }
}

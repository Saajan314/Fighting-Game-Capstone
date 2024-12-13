using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryBannerAnimation : MonoBehaviour
{
    public RectTransform banner;
    public float startSpeed = 10f;
    public float deceleration = 0.5f;
    public float endPositionX = 0f;
    public float targetPositionY = 0f;
    private Vector2 startPosition;
    private bool hasAnimationStarted = false;

    void Start()
    {
        // Only store the starting position
        startPosition = banner.anchoredPosition;
    }

    public void StartBannerAnimation()
    {
        if (!hasAnimationStarted)
        {
            hasAnimationStarted = true;
            // Reset position before starting animation
            banner.anchoredPosition = startPosition;
            StartCoroutine(AnimateVictoryBanner());
        }
    }

    public IEnumerator AnimateVictoryBanner()
    {
        Debug.Log("Banner animation started");
        float speed = startSpeed;
        float targetX = endPositionX;
        Vector2 targetPosition = new Vector2(targetX, targetPositionY);

        while (Vector2.Distance(banner.anchoredPosition, targetPosition) > 1f)
        {
            banner.anchoredPosition = Vector2.Lerp(banner.anchoredPosition, targetPosition, Time.deltaTime * speed);
            speed -= deceleration * Time.deltaTime;

            if (speed < 0f)
                speed = 0f;

            yield return null;
        }

        banner.anchoredPosition = targetPosition;
        Debug.Log("Banner animation completed");
    }
}




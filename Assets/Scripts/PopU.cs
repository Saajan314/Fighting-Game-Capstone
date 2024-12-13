using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopU : MonoBehaviour
{
    public GameObject popupPanel; // Assign your popup Panel in the inspector
    public Button closeButton;    // Assign the close button in the inspector

    void Start()
    {
        // Hide the popup at the start
        popupPanel.SetActive(false);

        // Add listener to the close button
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}


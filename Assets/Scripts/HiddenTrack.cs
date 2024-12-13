using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTrack : MonoBehaviour
{
    public float followSpeed = 5f; // Speed 
    private Transform player;  

    void Update()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                Debug.Log("Player found!");
            }
        }

        if (player != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
    }
}

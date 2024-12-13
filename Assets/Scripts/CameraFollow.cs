using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // player
    public float distance = 5.0f; // distance
    public float height = 3.0f; // height
    public float rotationSpeed = 100.0f; // speed
    public float verticalLimit = 60.0f; // vert

    private float currentYaw = 0.0f; // horz
    private float currentPitch = 0.0f; // vert

    void LateUpdate()
    {
        if (player == null) return;

        // right click
        if (Input.GetMouseButton(1))
        {
            float mouseDeltaX = Input.GetAxis("Mouse X");
            float mouseDeltaY = Input.GetAxis("Mouse Y");

            // rotattion on inp
            currentYaw += mouseDeltaX * rotationSpeed * Time.deltaTime;
            currentPitch -= mouseDeltaY * rotationSpeed * Time.deltaTime;

            // clamp
            currentPitch = Mathf.Clamp(currentPitch, -verticalLimit, verticalLimit);
        }

        // calc new pos
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // set
        transform.position = player.position + offset + Vector3.up * height;
        transform.LookAt(player.position + Vector3.up * height * 0.5f);
    }
}







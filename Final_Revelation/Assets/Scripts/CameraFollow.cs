using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float cameraSpeed = 2.0f;

    void Start()
    {
        // Cache the transform of the player to avoid repeated calls to GameObject.FindWithTag
        GameObject playerGo = GameObject.FindWithTag("Player");
        playerTransform = playerGo.transform;

    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            // Calculate the new position
            Vector3 newPosition = new Vector3(
                Mathf.Lerp(transform.position.x, playerTransform.position.x, cameraSpeed * Time.deltaTime),
                Mathf.Lerp(transform.position.y, playerTransform.position.y, cameraSpeed * Time.deltaTime),
                transform.position.z // Keep the camera's original Z position
            );

            // Set the position of the camera
            transform.position = newPosition;
        }
    }
}
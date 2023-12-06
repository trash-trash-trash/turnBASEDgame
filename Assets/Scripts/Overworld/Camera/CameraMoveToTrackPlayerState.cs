using UnityEngine;

public class CameraMoveToTrackPlayerState : MonoBehaviour
{
    public CameraBrain camBrain;
    public Transform camTransform;

    private float camSpeed;
    private Transform playerTransform;

    //increase speed to compensate for player movement
    public float speedMultiplier;
    
    private void OnEnable()
    {
        playerTransform = PlayerBrain.Instance.transform;
    }

    void Update()
    {
        // Calculate the direction from the camera to the player
        Vector3 directionToPlayer = playerTransform.position - camTransform.position;

        // Exclude the x and z components
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        // Calculate the limited rotation
        Quaternion limitedRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

        // Set the maximum angle between the current and limited rotation
        float maxRotationAngle = speedMultiplier * Time.deltaTime;

        // Smoothly rotate towards the limited rotation with a fixed speed
        camTransform.rotation = Quaternion.RotateTowards(camTransform.rotation, limitedRotation, maxRotationAngle);

        // Check if the rotation closely matches the limited rotation
        if (Quaternion.Angle(camTransform.rotation, limitedRotation) < 0.1f)
        {
            // Ensure the final rotation is exactly the limited rotation
            camTransform.rotation = limitedRotation;

            // Change state when it reaches the limited rotation
            camBrain.ChangeState(camBrain.camTrackPlayerState);
        }
    }
}
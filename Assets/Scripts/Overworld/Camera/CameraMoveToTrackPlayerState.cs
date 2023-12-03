using UnityEngine;

public class CameraMoveToTrackPlayerState : MonoBehaviour
{
    public CameraBrain camBrain;
    public Transform camTransform;

    private float camMaxAngle;
    private float camSpeed;
    private Transform playerTransform;

    private void OnEnable()
    {
        camMaxAngle = camBrain.maxAngle;
        camSpeed = camBrain.camMoveSpeed;
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

        // Limit the rotation if the angle exceeds the camMaxAngle
        float angleToPlayer = Vector3.Angle(camTransform.forward, directionToPlayer);
        if (angleToPlayer > camMaxAngle)
        {
            // Stay at the limited rotation, only rotating along the y-axis
            Vector3 limitedEulerAngles = limitedRotation.eulerAngles;
            limitedEulerAngles.x = 0f;
            limitedEulerAngles.z = 0f;
            limitedRotation = Quaternion.Euler(limitedEulerAngles);

            camTransform.rotation = Quaternion.RotateTowards(camTransform.rotation, limitedRotation,
                Time.deltaTime * camSpeed * 5);
        }
        else
        {
            // Smoothly rotate towards the limited rotation with a fixed speed
            camTransform.rotation = Quaternion.Lerp(camTransform.rotation, limitedRotation, Time.deltaTime * camSpeed *5);
        }

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
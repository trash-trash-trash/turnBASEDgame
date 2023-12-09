using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePlayerDetectState : MonoBehaviour
{
    public Transform pupilTransform;

    public Transform playerTransform;

    public float circleRadius;

    public Vector3 circleCentre;

    public float eyeMoveSpeed;

    private IEnumerator coRoFollowPlayer;
    
    void OnEnable()
    {
        circleCentre=transform.position;
        playerTransform = PlayerBrain.Instance.transform;
        coRoFollowPlayer = FollowPlayer();
        StartCoroutine(coRoFollowPlayer);
        
    }

    //pupilTransform may not move beyond circle radius from circleCentre
    //pupilTransform moves towards playerTransform along the x/y axis * eyeMoveSpeed. 
    
    IEnumerator FollowPlayer()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            Vector3 directionToPlayer = (new Vector3(playerTransform.position.x, playerTransform.position.y, 0) - new Vector3(pupilTransform.position.x, pupilTransform.position.y, 0)).normalized;

            // Project the target position onto the x-y plane to avoid movement along the z-axis
            Vector3 targetPosition = circleCentre + Vector3.ClampMagnitude(new Vector3(directionToPlayer.x, directionToPlayer.y, 0) * circleRadius, circleRadius);

            pupilTransform.position = Vector3.Lerp(
                pupilTransform.position,
                targetPosition,
                eyeMoveSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void OnDisable()
    {
        StopCoroutine(coRoFollowPlayer);
    }
}

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
            
            Vector3 directionToPlayer = (playerTransform.position - pupilTransform.position).normalized;

            Vector3 targetPosition = circleCentre + Vector3.ClampMagnitude(directionToPlayer * circleRadius, circleRadius);

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

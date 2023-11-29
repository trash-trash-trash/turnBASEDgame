using System.Collections;
using UnityEngine;

public class NPCChaseState : WalkerBase
{
    public VisionBase visionBase;
    public float downTime;

    public NPCBrain brain;

    private Coroutine returnHomeCountdown;

    private void OnEnable()
    {
        Accelerate();

        visionBase.SeePlayerBoolEvent += HandleSeePlayerEvent;
    }

    void Update()
    {
        Vector3 directionToPlayer = new Vector3(visionBase.playerTransform.position.x - walkerTransform.position.x, 0f, visionBase.playerTransform.position.z - walkerTransform.position.z);

        Vector3 normalizedDirection = directionToPlayer.normalized;

        movementVector2 = new Vector2(normalizedDirection.x, normalizedDirection.z);
    }

    private void HandleSeePlayerEvent(bool canSeePlayer)
    {
        if (canSeePlayer)
        {
            if (returnHomeCountdown != null)
            {
                StopCoroutine(returnHomeCountdown);
                returnHomeCountdown = null;
            }
        }
        else
        {
            if (returnHomeCountdown == null)
            {
                returnHomeCountdown = StartCoroutine(ReturnHomeCountdown());
            }
        }
    }

    IEnumerator ReturnHomeCountdown()
    {
        float timer = downTime;

        while (timer > 0)
        {
            yield return null; // Wait for one frame

            // If the player is seen, exit the coroutine and cancel the countdown
            if (visionBase.seePlayer)
            {
                returnHomeCountdown = null;
                yield break;
            }

            timer -= Time.deltaTime;
        }

        brain.ChangeState(OverworldNPCStates.Death);
    }

    private void OnDisable()
    {
        visionBase.SeePlayerBoolEvent -= HandleSeePlayerEvent;
    }
}
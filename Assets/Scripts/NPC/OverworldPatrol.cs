using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPatrol : WalkerBase
{
    public Talker talker;

    private ITalk iTalk;

    public List<Vector2> movementVectors = new List<Vector2>();
    public List<float> movementTime = new List<float>();
    public List<float> idleTimes = new List<float>();

    private int selectInt;
    private bool patrol;
    private float timeRemaining; // Time remaining on current movement

    private IEnumerator patrolCoroutine;
    private IEnumerator waitCoroutine;

    private void OnEnable()
    {
        iTalk = talker.GetComponent<ITalk>();
        iTalk.OpenDialogueEvent += StopPatrol;
        iTalk.CloseDialogueEvent += StartPatrol;

        StartPatrol();
    }

    private void StartPatrol()
    {
        patrol = true;
        Accelerate();

        // Check if there's time remaining from a previous patrol segment
        if (timeRemaining > 0)
        {
            StartCoroutine(ResumePatrol());
        }
        else
        {
            StartCoroutine(Patrol());
        }
    }

    private void StopPatrol()
    {
        patrol = false;
        StopAllCoroutines();
        Brake();

        // Store the time remaining when patrol is interrupted
        timeRemaining = GetCurrentMovementTime();
    }

    private float GetCurrentMovementTime()
    {
        if (selectInt < movementTime.Count)
            return movementTime[selectInt];
        else
            return 0;
    }

    private IEnumerator Patrol()
    {
        while (patrol)
        {
            movementVector2 = movementVectors[selectInt];
            timeRemaining = GetCurrentMovementTime();

            yield return new WaitForSeconds(timeRemaining);

            Brake();

            float waitTime = idleTimes[selectInt];
            yield return new WaitForSeconds(waitTime);

            Accelerate();

            selectInt++;
            if (selectInt >= movementVectors.Count)
                selectInt = 0;
        }
    }

    private IEnumerator ResumePatrol()
    {
        // Resume patrol with the time remaining from the interrupted patrol segment
        yield return new WaitForSeconds(timeRemaining);
        Brake();

        float waitTime = idleTimes[selectInt];
        yield return new WaitForSeconds(waitTime);

        Accelerate();

        selectInt++;
        if (selectInt >= movementVectors.Count)
            selectInt = 0;

        timeRemaining = 0; // Reset the time remaining after resuming
        StartCoroutine(Patrol()); // Continue regular patrol
    }

    private void OnDisable()
    {
        iTalk.OpenDialogueEvent -= StopPatrol;
        iTalk.CloseDialogueEvent -= StartPatrol;
    }
}
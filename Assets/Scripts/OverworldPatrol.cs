using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPatrol : WalkerBase
{
    public Talker talker;

    private ITalk iTalk;

    public List<Vector2> movementVectors = new List<Vector2>();

    public List<float> movementTime = new List<float>();

    private float currentTime;

    public List<float> idleTimes = new List<float>();

    public int selectInt;

    public bool patrol;

    private IEnumerator patrolCoroutine;

    private IEnumerator waitCoroutine;

    private void Start()
    {
        iTalk = talker.GetComponent<ITalk>();
        iTalk.OpenDialogueEvent += StopPatrol;
        iTalk.CloseDialogueEvent += StartPatrol;

        patrol = true;
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        while (patrol)
        {
            movementVector2 = movementVectors[selectInt];
            float currentTime = movementTime[selectInt];

            yield return new WaitForSeconds(currentTime);

            Brake();

            float waitTime = idleTimes[selectInt];
            yield return new WaitForSeconds(waitTime);

            Accelerate();

            selectInt++;
            if (selectInt >= movementVectors.Count)
                selectInt = 0;
        }
    }

    private void StartPatrol()
    {
        patrol = true;
        Accelerate();
        StartCoroutine(Patrol());
    }

    private void StopPatrol()
    {
        patrol = false;
        StopAllCoroutines();
        Brake();
    }

    private void OnDisable()
    {
        iTalk.OpenDialogueEvent -= StopPatrol;
        iTalk.CloseDialogueEvent -= StartPatrol;
    }
}

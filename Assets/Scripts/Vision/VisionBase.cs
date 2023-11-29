using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBase : MonoBehaviour
{
    public bool canSee;

    public int numberOfRaycasts;
    public float raycastDistance;
    public float angle;

    public event Action<bool> SeePlayerBoolEvent;

    public Transform playerTransform;

    public bool seePlayer = false;
    private bool prevSeePlayer = false;

    private void OnEnable()
    {
        ChangeSight(true);
    }

    void Update()
    {
        if (canSee)
        {
            for (int i = 0; i < numberOfRaycasts; i++)
            {
                // Calculate the angle for this raycast.
                float currentAngle;
                if (i == 0)
                {
                    currentAngle = 0f; // First raycast goes straight forward.
                }
                else if (i % 2 == 0)
                {
                    // Positive angle for even-indexed raycasts.
                    currentAngle = angle * (i / 2);
                }
                else
                {
                    // Negative angle for odd-indexed raycasts.
                    currentAngle = -angle * ((i - 1) / 2);
                }

                Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);

                // Calculate the direction for the raycast.
                Vector3 direction = rotation * transform.forward;

                Ray ray = new Ray(transform.position, direction);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, raycastDistance))
                {
                    IPlayer player;
                    if (hit.transform.GetComponent<IPlayer>() != null)
                    {
                        seePlayer = true;
                        playerTransform = hit.transform;
                    }
                }
                else
                {
                    seePlayer = false;
                    playerTransform = null;
                }
            }

            if (seePlayer != prevSeePlayer)
            {
                prevSeePlayer = seePlayer;
                SeePlayerBoolEvent?.Invoke(seePlayer);
            }
        }
    }

    public void ChangeSight(bool input)
    {
        canSee = input;
    }
}
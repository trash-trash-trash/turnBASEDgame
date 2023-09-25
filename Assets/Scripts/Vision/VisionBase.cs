using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBase : MonoBehaviour
{
    public bool canSee;

    public int numberOfRaycasts;
    public float raycastDistance;
    public float angle;

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
                        Debug.Log("Player spotted");
                }
            }
        }
    }

    public void ChangeSight(bool input)
    {
        canSee = input;
    }
}
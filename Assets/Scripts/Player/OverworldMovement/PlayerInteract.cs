using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //listens to PlayerControls events for Movement and Interact
    //Reads movement to determine what angle to fire the raycast at. If movement is 0, uses the last vector read
    //Fires a sphere cast the looks for ITalk at the raycast hit point
    //Note: changed raycast to line as it shortened when it hit something

    public PlayerControls controls;

    public PlayerOverworldMovement movement;

    private Vector2 aimVector;

    private Vector3 lastVector;

    private Vector3 direction;

    public float lineLength;

    public float interactSphereRadius;

    public bool talking = false;

    private void OnEnable()
    {
        controls.MovementEvent += AimVector;
        controls.InteractEvent += Interact;
    }

    private void OnDisable()
    {
        controls.MovementEvent -= AimVector;
        controls.InteractEvent -= Interact;
    }

    private void AimVector(Vector2 vector2)
    {
        aimVector = vector2;

        direction = new Vector3(aimVector.x, 0f, aimVector.y);

        if (direction != Vector3.zero)
            lastVector = direction;

        if (direction == Vector3.zero)
            direction = lastVector;
    }

    private void Interact()
    {
        if (talking)
        {
            Debug.Log("Interact Script");
            DialogueSingleton.Instance.OnOpenCloseDialogue(false);
            movement.canMove = true;
            talking = false;
            return;
        }

        Vector3 origin = transform.position;

        // Calculate the end point based on a custom length.
        Vector3 endPosition = origin + direction.normalized * lineLength;

        // Create an array to store the hits.
        RaycastHit[] hits = Physics.SphereCastAll(origin, interactSphereRadius, direction, lineLength);

        // Check if any hits were detected.
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                ITalk talker = hit.collider.gameObject.GetComponent<ITalk>();
                if (talker != null)
                {
                    talker.OpenDialogue();
                    movement.Brake();
                    talking = true;
                    break; // Exit the loop after the first hit.
                }
            }
        }
        else
        {
            Debug.DrawRay(origin, direction.normalized * lineLength, Color.green);
        }
    }
}
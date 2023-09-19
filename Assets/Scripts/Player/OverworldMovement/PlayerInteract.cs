using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //listens to PlayerControls events for Movement and Interact
    //Reads movement to determine what angle to fire the raycast at. If movement is 0, uses the last vector read
    //Fires a sphere cast the looks for ITalk at the raycast hit point

    public PlayerControls controls;

    public PlayerOverworldMovement movement;

    private Vector2 aimVector;

    private Vector3 lastVector;

    private Vector3 direction;

    public float raycastLength;

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

        Vector3 endPosition = origin + direction * raycastLength;

        RaycastHit hitInfo;
        bool hit = Physics.SphereCast(origin, interactSphereRadius, direction, out hitInfo, interactSphereRadius);

        if (hit)
        {
            Debug.DrawLine(origin, hitInfo.point, Color.red);

            ITalk talker = hitInfo.collider.gameObject.GetComponent<ITalk>();
            if (talker != null)
            {
                talker.OpenDialogue();
                movement.Brake();
                talking = true;
            }
        }
        else
        {
            Debug.DrawRay(origin, direction * interactSphereRadius, Color.green);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerBase : MonoBehaviour
{
    public Rigidbody rb;

    public Vector2 movementVector2;

    public bool canMove;

    public float maxSpeed;

    public float walkSpeed;  
    
    public float brakeForce;

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 movementDirection = new Vector3(movementVector2.x, 0f, movementVector2.y);

            if (movementDirection != Vector3.zero)
            {
                rb.AddForce(movementDirection * walkSpeed, ForceMode.VelocityChange);

                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
            else
            {
                Vector3 currentVelocity = rb.velocity;
                Vector3 dampingForce = -currentVelocity * brakeForce;
                rb.AddForce(dampingForce, ForceMode.Acceleration);
            }
        }
    }

    public void Accelerate()
    {
        canMove = true;
    }

    public void Brake()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
    }
}

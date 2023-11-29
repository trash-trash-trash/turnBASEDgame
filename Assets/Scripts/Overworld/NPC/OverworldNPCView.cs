using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldNPCView : MonoBehaviour
{
    public WalkerBase walker;

    public SpriteRenderer spr;

    public Animator animator;

    public AnimationClip walkUp;

    public AnimationClip walkDown;

    public AnimationClip walkSide;

    public AnimationClip idleUp;

    public AnimationClip idleDown;

    public AnimationClip idleSide;

    public Vector3 prevVector=Vector3.zero;

    private bool isMoving;

    public LookTowards lookTowards;

    public void OnEnable()
    {
        animator.SetLayerWeight(0, 1f);

        walker.MovementVector3Event += Movement;
        lookTowards.AnnounceVisionTargetEvent += Movement;
    }

    private void Movement(Vector3 vec)
    {
        if (vec == Vector3.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
            prevVector = vec;
        }

        if (isMoving)
        {
            if (vec.z > 0)
            {
                animator.Play(walkUp.name);
            }
            else if (vec.z < 0)
            {
                animator.Play(walkDown.name);
            }
            else if (vec.x != 0)
            {
                animator.Play(walkSide.name);
            }

            if (vec.x < 0)
            {
                spr.flipX = true;
            }
            else if (vec.x > 0)
            {
                spr.flipX = false;
            }
        }
        else
        {
            if (prevVector.z > 0)
            {
                animator.Play(idleUp.name);
            }
            else if (prevVector.z < 0)
            {
                animator.Play(idleDown.name);
            }
            else
            {
                animator.Play(idleSide.name);
            }
        }
    }
    
    void OnDisable()
    {
        walker.MovementVector3Event -= Movement;
        lookTowards.AnnounceVisionTargetEvent -= Movement;
    }
}
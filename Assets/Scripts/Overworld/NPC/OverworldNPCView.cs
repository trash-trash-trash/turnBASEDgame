using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldNPCView : MonoBehaviour
{
    public LookTowards lookTowards;

    public SpriteRenderer spr;

    public Animator animator;

    public AnimationClip walkUp;

    public AnimationClip walkDown;

    public AnimationClip walkSide;

    public void OnEnable()
    {
        lookTowards.AnnounceVisionTargetEvent += Movement;
    }

    private void Movement(Vector3 vec)
    {
        Debug.Log(vec);

        if (vec.z > 0)
        {
            animator.Play(walkUp.name);
        }
        else if (vec.z < 0)
        {
            animator.Play(walkDown.name);
        }
        else
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
    void OnDisable()
    {
        lookTowards.AnnounceVisionTargetEvent -= Movement;
    }
}
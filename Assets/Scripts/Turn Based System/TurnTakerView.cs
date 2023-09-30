using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TurnTakerView : MonoBehaviour
{
    //seems redundant to hold so many dictionaries? better way?
    //better key than string partyMember name

    public AnimatorController currentController;

    public Animator animator;

    public AnimatorController clericController;

    public AnimatorController castradoController;

    public Dictionary<string, AnimatorController> partyControllers =
        new Dictionary<string, AnimatorController>();

    public TurnTaker turnTaker;

    public SpriteRenderer spr;

    public Material highlightedMat;

    public Material defaultMat;

    public void OnEnable()
    {
        partyControllers.Add("Cleric", clericController);
        partyControllers.Add("Castrado", castradoController);

        turnTaker.DeclareHighlightedEvent += Highlight;
        turnTaker.DeclareStartTurnEvent += StartTurn;

        Highlight(false);
    }

    public void Highlight(bool input)
    {
        if (input)
        {
            spr.material = highlightedMat;
        }

        else
        {
            spr.material = defaultMat;
        }
    }

    public void StartTurn()
    {
        //!!!HACK WARNING HACK!!!
        if (partyControllers.ContainsKey(turnTaker.transform.name))
        {
            currentController = partyControllers[turnTaker.transform.name];

            animator.runtimeAnimatorController = currentController;
            

            if (turnTaker.transform.name == "Cleric")
            {
                animator.Play("TurnBasedPortrait");
            }
            else if (turnTaker.transform.name == "Castrado")
            {/*
                int randomValue = Random.Range(0, 2);
                
                // Use a ternary operator to choose between two code blocks
                string newString = (randomValue == 0) ? "TurnBasedPortait01" : "TurnBasedPortrait02";

                animator.Play(newString);*/
                animator.Play("TurnBasedPortrait01");
            }
        }
    }
    

    public void OnDisable()
    {
        turnTaker.DeclareHighlightedEvent -= Highlight;
        turnTaker.DeclareStartTurnEvent -= StartTurn;
    }
}

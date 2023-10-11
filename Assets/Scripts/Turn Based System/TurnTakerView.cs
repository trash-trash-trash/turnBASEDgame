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

    public Dictionary<PartyMemberScriptableObject, AnimatorController> partyControllers =
        new Dictionary<PartyMemberScriptableObject, AnimatorController>();

    public TurnTaker turnTaker;

    public SpriteRenderer spr;

    public Material highlightedMat;

    public Material defaultMat;

    public TurnTakerDictionary ttDict;

    public StatsBase statBase;

    public void OnEnable()
    {
        ttDict = TurnTakerDictionary.TurnTakerDictionaryInstance;

        partyControllers.Add(ttDict.clericSO, clericController);

        partyControllers.Add(ttDict.castradoSO, castradoController);

        turnTaker.DeclareHighlightedEvent += Highlight;

        Highlight(false);

        StartTurn();
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
        if (partyControllers.ContainsKey(statBase.member))
        {
            currentController = partyControllers[statBase.member];

            animator.runtimeAnimatorController = currentController;

            if (turnTaker.transform.name == "Cleric")
            {
                animator.Play("TurnBasedPortrait");
            }

            else if (turnTaker.transform.name == "Castrado")
            {
                int randomValue = Random.Range(0, 2);
                
                // Use a ternary operator to choose between two code blocks
                string newString = (randomValue == 0) ? "TurnBasedPortrait01" : "TurnBasedPortrait02";

                animator.Play(newString);
            }
        }
    }
    

    public void OnDisable()
    {
        turnTaker.DeclareHighlightedEvent -= Highlight;
    }
}

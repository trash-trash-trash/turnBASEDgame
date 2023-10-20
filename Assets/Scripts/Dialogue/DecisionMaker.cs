using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaker : MonoBehaviour, IInteractable
{
    public List<string> decisionOptions;

    public bool interacting = false;

    public void SetDecision()
    {

    }

    public void Interact()
    {
        if (interacting)
            return;
        
        OpenDecisions();
    }

    void OpenDecisions()
    {
        interacting = true;
    }
}

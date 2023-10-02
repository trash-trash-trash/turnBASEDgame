using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTaker : MonoBehaviour, ITakeTurn
{
    public bool turnTaken;  
    public bool TurnTaken()
    {
        return turnTaken;
    }

    public event Action<TurnTaker, bool> DeclareTurnTakenEvent;
    
    public event Action <bool>DeclareHighlightedEvent;

    public void StartTurn()
    {
        turnTaken = false;
        AnnouncePlayerReady(this, false);
    }


    public void SetHighlighted(bool input)
    {
        DeclareHighlightedEvent?.Invoke(input);
    }

    public void EndTurn()
    {
        turnTaken = true;
        AnnouncePlayerReady(this, true);
    }

    public void AnnouncePlayerReady(TurnTaker turnInput, bool input)
    {
        DeclareTurnTakenEvent?.Invoke(turnInput, input);
    }
}

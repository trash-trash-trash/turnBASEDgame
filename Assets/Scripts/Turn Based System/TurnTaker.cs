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

    public event Action DeclareStartTurnEvent;
    public event Action <bool>DeclareHighlightedEvent;

    public event Action<TurnTaker, bool> PlayerReadyEvent;

    public bool itsMyTurn;

    public bool ItsMyTurn()
    {
        return itsMyTurn;
    }

    public bool turnLocked;

    public bool TurnLocked()
    {
        return turnLocked;
    }

    public void SetHighlighted(bool input)
    {
        DeclareHighlightedEvent?.Invoke(input);
    }

    public void SetItsMyTurn(bool input)
    {
        itsMyTurn = input;

        if (input)
            StartTurn();
    }

    public void SetTurnLocked(bool input)
    {
        turnLocked = input;
    }

    public void StartTurn()
    {
        turnTaken = false;
        DeclareStartTurnEvent?.Invoke();
    }

    public void EndTurn()
    {
        turnTaken = true;
        AnnouncePlayerReady(this, true);
    }

    public void AnnouncePlayerReady(TurnTaker turnInput, bool input)
    {
        PlayerReadyEvent?.Invoke(turnInput, input);
    }
}

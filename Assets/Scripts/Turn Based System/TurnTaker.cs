using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTaker : MonoBehaviour, ITakeTurn
{
    public TurnTakerID turnTakerID;

    public bool turnTaken;

    public bool TurnTaken()
    {
        return turnTaken;
    }

    public event Action DeclareStartTurnEvent;

    public event Action<TurnTakerID, bool> PlayerReadyEvent;

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
        AnnouncePlayerReady(turnTakerID, true);
    }

    public void AnnouncePlayerReady(TurnTakerID turnInput, bool input)
    {
        PlayerReadyEvent?.Invoke(turnInput, input);
    }
}

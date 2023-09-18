using System;

public interface ITakeTurn
{
    public bool ItsMyTurn();

    public bool TurnLocked();

    public bool TurnTaken();

    public event Action<TurnTakerID, bool> PlayerReadyEvent;

    public event Action DeclareStartTurnEvent;

    public void SetItsMyTurn(bool input);

    public void SetTurnLocked(bool input);

    public void EndTurn();
}
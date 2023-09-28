using System;

public interface ITakeTurn
{
    public event Action<TurnTaker, bool> PlayerReadyEvent;

    public event Action DeclareStartTurnEvent;

    public event Action DeclareHighlightedEvent;

    public bool ItsMyTurn();

    public bool TurnLocked();

    public bool TurnTaken();

    public void SetItsMyTurn(bool input);

    public void SetTurnLocked(bool input);

    public void EndTurn();
}
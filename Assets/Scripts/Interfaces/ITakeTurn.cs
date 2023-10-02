using System;

public interface ITakeTurn
{
    public event Action<TurnTaker, bool> DeclareTurnTakenEvent;

    public event Action <bool>DeclareHighlightedEvent;

    public bool TurnTaken();

    public void StartTurn();

    public void EndTurn();
}
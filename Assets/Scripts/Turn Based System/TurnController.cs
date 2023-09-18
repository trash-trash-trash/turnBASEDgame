using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Analytics;

public class TurnController : MonoBehaviour
{
    public GameObject dealerTurn;

    public GameObject playerTurn;

    public GameObject dealerObj;
    public ITakeTurn dealer;

    public GameObject playerOneObj;
    public ITakeTurn playerOne;

    public GameObject playerTwoObj;
    public ITakeTurn playerTwo;

    public Dictionary<TurnTakerID, ITakeTurn> ITakeTurnDictionary = new Dictionary<TurnTakerID, ITakeTurn>();

    public event Action<string> AnnounceControllerString;

    public void Awake()
    {
        dealer = dealerObj.GetComponent<ITakeTurn>();
        playerOne = playerOneObj.GetComponent<ITakeTurn>();
        playerTwo = playerTwoObj.GetComponent<ITakeTurn>();

        ITakeTurnDictionary.Add(TurnTakerID.Dealer, dealer);
        ITakeTurnDictionary.Add(TurnTakerID.PlayerOne, playerOne);
        ITakeTurnDictionary.Add(TurnTakerID.PlayerTwo, playerTwo);

        dealer.PlayerReadyEvent += DealerReady;
        playerOne.PlayerReadyEvent += PlayerReady;
        playerTwo.PlayerReadyEvent += PlayerReady;

        PlayerTurn();
    }

    public void SetControllerText(string input)
    {
        AnnounceControllerString?.Invoke(input);
    }

    public void PlayerReady(TurnTakerID turnTakerID, bool b)
    {
        SetControllerText("Player's turn\n" + turnTakerID + " ended turn");

        if (playerOne.TurnTaken() && playerTwo.TurnTaken())
            DealerTurn();
    }

    public void PlayerTurn()
    {
        dealer.SetTurnLocked(true);
        playerOne.SetTurnLocked(false);
        playerTwo.SetTurnLocked(false);

        dealer.SetItsMyTurn(false);
        playerOne.SetItsMyTurn(true);
        playerTwo.SetItsMyTurn(true);

        SetControllerText("Player's turn");
    }

    public void DealerReady(TurnTakerID turnTakerId, bool input)
    {
        if (input)
            PlayerTurn();
    }

    public void DealerTurn()
    {
        dealer.SetTurnLocked(false);
        playerOne.SetTurnLocked(true);
        playerTwo.SetTurnLocked(true);

        dealer.SetItsMyTurn(true);
        playerOne.SetItsMyTurn(false);
        playerTwo.SetItsMyTurn(false);

        SetControllerText("Dealer's Turn");
    }

    public void OnDisable()
    {
        dealer.PlayerReadyEvent -= DealerReady;
        playerOne.PlayerReadyEvent -= PlayerReady;
        playerTwo.PlayerReadyEvent -= PlayerReady;
    }
}
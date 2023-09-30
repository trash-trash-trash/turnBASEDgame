using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using JetBrains.Annotations;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class TurnController : MonoBehaviour
{
    //player Two will have to be an AI
    public PartyController playerOnePartyController;
    public List<PartyMemberScriptableObject> playerOneParty;
    public List<Transform> partyOneObjSpawnPoints;

    public List<PartyMemberScriptableObject> playerTwoParty;
    public List<Transform> partyTwoObjSpawnPoints;

    public GameObject playerOneObj;
    public GameObject playerTwoObj;

    public GameObject dealerObj;
    public ITakeTurn dealer;

    public Dictionary<TurnTaker, bool> playerOneTurnTakersDict = new Dictionary<TurnTaker, bool>();
    public Dictionary<TurnTaker, bool> playerTwoTurnTakersDict = new Dictionary<TurnTaker, bool>();

    public event Action<string> AnnounceControllerString;

    public BattleManagerSingleton battleManager;

    public GameObject turnTakerPrefab;

    public void Awake()
    {
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        playerOneParty = battleManager.playerOneParty;
        playerTwoParty = battleManager.playerTwoParty;

        dealer = dealerObj.GetComponent<ITakeTurn>();

        dealer.PlayerReadyEvent += DealerReady;

        StartGame();
    }

    private void StartGame()
    {
        for (int i = 0; i < playerOneParty.Count; i++)
        {
            PartyMemberScriptableObject member = playerOneParty[i];
            Transform spawnPoint = partyOneObjSpawnPoints[i]; // Get the corresponding spawn point

            turnTakerPrefab = Instantiate(turnTakerPrefab, spawnPoint.position, Quaternion.identity);

            StatsBase stats = turnTakerPrefab.GetComponent<StatsBase>();
            stats.member = member;
            stats.Initialize();

            turnTakerPrefab.name = member.name;
            turnTakerPrefab.transform.SetParent(playerOneObj.transform);

            TurnTaker taker = turnTakerPrefab.GetComponent<TurnTaker>();
            taker.PlayerReadyEvent += PlayerReady;

            playerOneTurnTakersDict.Add(taker, false);
        }

        for (int i = 0; i < playerTwoParty.Count; i++)
        {
            PartyMemberScriptableObject member = playerTwoParty[i];
            Transform spawnPoint = partyTwoObjSpawnPoints[i];

            turnTakerPrefab = Instantiate(turnTakerPrefab, spawnPoint.position, Quaternion.identity);

            StatsBase stats = turnTakerPrefab.GetComponent<StatsBase>();
            stats.member = member;
            stats.Initialize();

            turnTakerPrefab.name = member.name;
            turnTakerPrefab.transform.SetParent(playerTwoObj.transform);

            TurnTaker taker = turnTakerPrefab.GetComponent<TurnTaker>();
            taker.PlayerReadyEvent += PlayerReady;

            playerTwoTurnTakersDict.Add(taker, false);
        }

        PlayerTurn();
    }

    public void SetControllerText(string input)
    {
        AnnounceControllerString?.Invoke(input);
    }

    public void PlayerReady(TurnTaker turnTaker, bool input)
    {
        if (playerOneTurnTakersDict.ContainsKey(turnTaker))
        {
            playerOneTurnTakersDict[turnTaker] = input;
        }

        else if (playerTwoTurnTakersDict.ContainsKey(turnTaker))
        {
            playerTwoTurnTakersDict[turnTaker] = input;
        }

        //for each thru each dictionary and return if any results are false
        //chat gpt says var kvp

        foreach (var kvp in playerOneTurnTakersDict)
        {
            if (!kvp.Value)
            {
                return;
            }
        }

        foreach (var kvp in playerTwoTurnTakersDict)
        {
            if (!kvp.Value)
            {
                return;
            }
        }

        DealerTurn();
    }

    public void SetShit(Dictionary<TurnTaker, bool> targetDict, bool turnLocked, bool itsMyTurn)
    {
        foreach (TurnTaker member in targetDict.Keys)
        {
            member.SetTurnLocked(turnLocked);
            member.SetItsMyTurn(itsMyTurn);
        }
    }

    public void PlayerTurn()
    {
        playerOnePartyController.SetParty(playerOneParty, playerTwoParty, new List<TurnTaker>(playerOneTurnTakersDict.Keys), new List<TurnTaker> (playerTwoTurnTakersDict.Keys));

        dealer.SetTurnLocked(true);
        dealer.SetItsMyTurn(false);

        SetShit(playerOneTurnTakersDict, false, true);
        SetShit(playerTwoTurnTakersDict, false, true);

        SetControllerText("Player's turn");
    }

    public void DealerReady(TurnTaker turnTakerId, bool input)
    {
        if (input)
            PlayerTurn();
    }

    public void DealerTurn()
    {        
        dealer.SetItsMyTurn(true);
        dealer.SetTurnLocked(false);
        
        SetShit(playerOneTurnTakersDict, true, false);
        SetShit(playerTwoTurnTakersDict, true, false);

        SetControllerText("Dealer's Turn");
    }

    public void EndFight()
    {
        BattleManagerSingleton battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        battleManager.EndFight();
    }

    public void OnDisable()
    {
        dealer.PlayerReadyEvent -= DealerReady;

        foreach (TurnTaker member in playerOneTurnTakersDict.Keys)
        {
            member.PlayerReadyEvent -= PlayerReady;
        }

        foreach (TurnTaker member in playerTwoTurnTakersDict.Keys)
        {
            member.PlayerReadyEvent -= PlayerReady;
        }

        playerOneTurnTakersDict.Clear();
        playerTwoTurnTakersDict.Clear();
    }
}
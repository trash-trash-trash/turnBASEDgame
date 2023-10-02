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

    public event Action<string, TurnTakerID> AnnounceControllerString;

    public event Action<List<PartyMemberScriptableObject>, List<PartyMemberScriptableObject>, List<TurnTaker>,
        List<TurnTaker>> SetPartyEvent;

    public event Action PlayerTurnStartedEvent;

    public event Action<TurnTakerID, bool> DeclareIDTurnStatusEvent;

    public BattleManagerSingleton battleManager;

    public GameObject turnTakerPrefab;

    public ActionsSingleton actionsSingleton;

    public void Awake()
    {
        battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        playerOneParty = battleManager.playerOneParty;
        playerTwoParty = battleManager.playerTwoParty;

        dealer = dealerObj.GetComponent<ITakeTurn>();

        dealer.DeclareTurnTakenEvent += DealerReady;

        actionsSingleton = ActionsSingleton.Instance;

        StartCoroutine(WaitForActionsDictionary());
    }

    private IEnumerator WaitForActionsDictionary()
    {
        while (!actionsSingleton.initialized)
            yield return null;

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
            member.stats = stats;
            stats.Initialize();

            turnTakerPrefab.name = member.name;
            turnTakerPrefab.transform.SetParent(playerOneObj.transform);

            TurnTaker taker = turnTakerPrefab.GetComponent<TurnTaker>();
            taker.DeclareTurnTakenEvent += PlayerReady;

            taker.StartTurn();

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
            taker.DeclareTurnTakenEvent += PlayerReady;

            taker.StartTurn();

            playerTwoTurnTakersDict.Add(taker, false);
        }

        DeclareIDTurnStatusEvent?.Invoke(TurnTakerID.PlayerOne, false);
        DeclareIDTurnStatusEvent(TurnTakerID.PlayerTwo, false);
        DeclareIDTurnStatusEvent(TurnTakerID.Dealer, true);

        PlayerTurn();
    }

    public void SetControllerText(string input, TurnTakerID ID)
    {
        AnnounceControllerString?.Invoke(input, ID);
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

    public void PlayerTurn()
    {
        SetPartyEvent?.Invoke(playerOneParty, playerTwoParty, new List<TurnTaker>(playerOneTurnTakersDict.Keys),
            new List<TurnTaker>(playerTwoTurnTakersDict.Keys));
        
        PlayerTurnStartedEvent?.Invoke();
    }

    public void DealerReady(TurnTaker turnTakerId, bool input)
    {
        StartCoroutine(HackWait());
    }

    private IEnumerator HackWait()
    {
        SetControllerText("Your fate wavers uncertainly...", TurnTakerID.PlayerOne);
        SetControllerText("Your fate wavers uncertainly...", TurnTakerID.PlayerTwo);
        yield return new WaitForSeconds(3);
        PlayerTurn();
    }

    public void DealerTurn()
    {
        DeclareIDTurnStatusEvent?.Invoke(TurnTakerID.PlayerOne, true);
        DeclareIDTurnStatusEvent(TurnTakerID.PlayerTwo, true);
        DeclareIDTurnStatusEvent(TurnTakerID.Dealer, false);
    }

    public void EndFight()
    {
        BattleManagerSingleton battleManager = BattleManagerSingleton.BattleManagerSingletonInstance;
        battleManager.EndFight();
    }

    public void OnDisable()
    {
        dealer.DeclareTurnTakenEvent -= DealerReady;

        foreach (TurnTaker member in playerOneTurnTakersDict.Keys)
        {
            member.DeclareTurnTakenEvent -= PlayerReady;
        }

        foreach (TurnTaker member in playerTwoTurnTakersDict.Keys)
        {
            member.DeclareTurnTakenEvent -= PlayerReady;
        }

        playerOneTurnTakersDict.Clear();
        playerTwoTurnTakersDict.Clear();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTakerDictionary : MonoBehaviour
{

    #region Singleton

    private static TurnTakerDictionary instance;

    public static TurnTakerDictionary TurnTakerDictionaryInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TurnTakerDictionary>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("singleton");
                    instance = singletonObject.AddComponent<TurnTakerDictionary>();
                }
            }

            return instance;
        }
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion

    public Dictionary<PartyMemberScriptableObject, GameObject> turnTakerDictionary = new Dictionary<PartyMemberScriptableObject, GameObject>();

    [Header("Player Characters")]

    public PartyMemberScriptableObject clericSO;
    public GameObject clericGO;

    [Header("Enemy NPCs")] 
    
    public PartyMemberScriptableObject castradoSO;
    public GameObject castradoGO;

    private GameObject newTurnTaker;

    public void OnEnable()
    {
        turnTakerDictionary.Add(clericSO, clericGO);

        turnTakerDictionary.Add(castradoSO, castradoGO);
    }

    public GameObject SpawnTurnTaker(PartyMemberScriptableObject so)
    {
        if (turnTakerDictionary.ContainsKey(so))
        {
            newTurnTaker = Instantiate(turnTakerDictionary[so]) as GameObject;
        }

        return newTurnTaker;
    }
}

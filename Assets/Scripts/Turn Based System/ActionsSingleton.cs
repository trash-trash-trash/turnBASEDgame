using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsSingleton : MonoBehaviour
{
    private static ActionsSingleton instance;
    
    public Dictionary<int, Actions> actionsDict = new Dictionary<int, Actions>();

    public bool initialized = false;
    
    public enum Actions
    {
        Attack,
        Spells,
        Items,
        Guard,
        Flee
    }
    
    public static ActionsSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ActionsSingleton>();
                
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ActionsSingleton");
                    instance = singletonObject.AddComponent<ActionsSingleton>();
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
    
    private void Start()
    {
        actionsDict.Add(0, Actions.Attack);
        actionsDict.Add(1, Actions.Spells);
        actionsDict.Add(2, Actions.Items);
        actionsDict.Add(3, Actions.Guard);
        actionsDict.Add(4, Actions.Flee);
        initialized = true;
    }
}
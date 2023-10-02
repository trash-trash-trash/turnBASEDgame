using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPartyMemberScriptableObject", menuName = "PartyScriptableObject")]
public class PartyMemberScriptableObject : ScriptableObject
{
    public string name;

    public StatsBase stats;

    public ActionScriptableObject selectedAction;

    public List<ActionScriptableObject> actions;

    public string HiddenTurnDescription;

    public string ContemplateTurnDescription;

    public string description;

    public bool equipped = false;

    public int HP;
    public int Mind;    
    public int Speed;
    public int Accuracy;
    public int PhysDamage;
    public int PhysDefense;
    public int MagDamage;
    public int MagDefense;

    public int Luck;

    public Sprite combatSprite;
    //class?

    //change to set equipped Action
    public void OnEnable()
    {
        selectedAction = actions[0];
    }
}
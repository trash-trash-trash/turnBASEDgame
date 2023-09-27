using UnityEngine;

[CreateAssetMenu(fileName = "NewPartyMemberScriptableObject", menuName = "PartyScriptableObject")]
public class PartyMemberScriptableObject : ScriptableObject
{
    public string name;

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

    //class?
}
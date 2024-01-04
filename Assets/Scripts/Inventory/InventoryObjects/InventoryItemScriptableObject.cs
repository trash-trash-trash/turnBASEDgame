using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItemScriptableObject", menuName = "InventoryItemScriptableObject")]
public class InventoryItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    
    public string name;

    public string description;

    public List<ItemStatsEffected> statsList = new List<ItemStatsEffected>();

    public Sprite itemSprite;
    
    [System.Serializable]
    public class ItemStatsEffected
    {
        public StatsEnum stat;
        public int statVariable;
    }

}
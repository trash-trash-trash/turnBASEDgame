using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitsInventory : MonoBehaviour
{
    public StatsBase stats;

    public TurnTaker turnTaker;

    [SerializeField]
    public List<GameObject> statusObjList = new List<GameObject>();

    public List<IStatus> statusList = new List<IStatus>();

    public void CheckTraits()
    {
        foreach (GameObject targetObj in statusObjList)
        {
            IStatus targetStatus = targetObj.GetComponent<IStatus>();
            targetStatus.SetOwner(stats);
            AddStatus(targetStatus);
        }
    }

    public void InflictStatus()
    {
        foreach (IStatus targetStatus in statusList)
        {
            targetStatus.SetStatus();
        }
    }

    public void AddStatus(IStatus input)
    {
        if(!statusList.Contains(input))
            statusList.Add(input);
    }

    public void RemoveStatus(IStatus input)
    {
        if(statusList.Contains(input))
            statusList.Remove(input);
    }

    public void OnDisable()
    {
    }
}

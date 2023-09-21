using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInventory : MonoBehaviour
{
    public GameObject partyPrefab;

    public PartyMemberScriptableObject mainMember;

    public List<PartyMemberScriptableObject> party = new List<PartyMemberScriptableObject>();

    private void Start()
    {
        SetStats();
    }

    public void SetStats()
    {
        foreach (PartyMemberScriptableObject partyMember in party)
        {
            GameObject newObj = Instantiate(partyPrefab);

            PartyMember newPartyMember = newObj.GetComponent<PartyMember>();
            newPartyMember.scriptableObject = partyMember;
            newPartyMember.SetStats();

            newObj.transform.SetParent(transform);
            newObj.name = partyMember.name;
        }
    }

    public void EquipMainMember()
    {
        ChangePartyLeader(mainMember);
    }

    public void ChangePartyLeader(PartyMemberScriptableObject input)
    {
        if (party.Contains(input))
        {
            // Remove the input party member from its current position (if it exists).
            party.Remove(input);
        }
        else
        {
            Debug.LogWarning("Party member not found in the party list.");
            return;
        }

        // Shuffle the party list.
        ShufflePartyList();

        // Add the input party member as the new leader (at the beginning of the list).
        party.Insert(0, input);
    }

    private void ShufflePartyList()
    {
        int n = party.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            PartyMemberScriptableObject temp = party[k];
            party[k] = party[n];
            party[n] = temp;
        }
    }
}
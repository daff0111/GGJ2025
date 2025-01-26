using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    List<Bubblemon> bubblemons;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Bubblemon> bubblemons)
    {
        this.bubblemons = bubblemons;
        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < bubblemons.Count)
                memberSlots[i].SetData(bubblemons[i]);
            else 
                memberSlots[i].gameObject.SetActive(false);
        }

        messageText.text = "Choose a Bubblemon";
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < bubblemons.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}

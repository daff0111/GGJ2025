using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    public PartyMemberUI[] memberSlots;
    List<Bubblemon> bubblemons;

    public void Init()
    {
        //memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Bubblemon> bubblemons)
    {
        this.bubblemons = bubblemons;
        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < bubblemons.Count)
            {
                memberSlots[i].SetData(bubblemons[i]);
                memberSlots[i].gameObject.SetActive(true); // Asegurar que el slot esté activo si tiene un Bubblemon
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false); // Desactivar slots vacíos
            }
        }

        messageText.text = "Choose a Bubblemon";
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < bubblemons.Count; i++) // Iterar sobre los slots
        {
            if (i < bubblemons.Count) // Asegurarse de que hay un Bubblemon para este slot
            {
                if (i == selectedMember)
                    memberSlots[i].SetSelected(true);
                else
                    memberSlots[i].SetSelected(false);
            }
        }

        // Validar que el índice seleccionado no exceda los límites
        if (selectedMember < 0 || selectedMember >= bubblemons.Count)
        {
            Debug.LogError($"Índice seleccionado inválido: {selectedMember}. Rango válido: 0 - {bubblemons.Count - 1}");
        }
    }

    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}

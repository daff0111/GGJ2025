using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color highlightedColor;

    Bubblemon _bubblemon;

    public void SetData(Bubblemon bubblemon)
    {
        _bubblemon = bubblemon;

        nameText.text = bubblemon.Base.Name; 
        levelText.text = "Lvl " + bubblemon.Level; 
        hpBar.SetHP((float) bubblemon.HP / bubblemon.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = highlightedColor;
        else
            nameText.color = Color.black;
    }
}

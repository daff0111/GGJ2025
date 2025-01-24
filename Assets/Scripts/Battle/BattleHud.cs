using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(Bubblemon bubblemon)
    {
        nameText.text = bubblemon.Base.Name; 
        levelText.text = "Lvl " + bubblemon.Level; 
        hpBar.SetHP((float) bubblemon.HP / bubblemon.MaxHp);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Bubblemon _bubblemon;

    public void SetData(Bubblemon bubblemon)
    {
        _bubblemon = bubblemon;

        nameText.text = bubblemon.Base.Name; 
        levelText.text = "Lvl " + bubblemon.Level; 
        hpBar.SetHP((float) bubblemon.HP / bubblemon.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float) _bubblemon.HP / _bubblemon.MaxHp);
    }
}

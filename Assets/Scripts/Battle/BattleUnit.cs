using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] BubblemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Bubblemon Bubblemon { get; set; }

    public void Setup()
    {
        Bubblemon = new Bubblemon(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = Bubblemon.Base.BackSprite;
        else
            GetComponent<Image>().sprite = Bubblemon.Base.FrontSprite;
    }
}

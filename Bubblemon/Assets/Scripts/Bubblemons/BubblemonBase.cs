using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bubblemon", menuName = "Bubblemon/Create new bubblemon")]
public class BubblemonBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] BubblemonType type1;
    [SerializeField] BubblemonType type2;

    // Base Stats
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    public string GetName()
    {
        return name;
    }

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return description; }
    }

    public Sprite FrontSprite {
        get { return frontSprite; }
    }

    public Sprite BackSprite {
        get { return backSprite; }
    }

    public BubblemonType Type1 {
        get { return type1; }
    }

    public BubblemonType Type2 {
        get { return type2; }
    }

    public int MaxHP {
        get { return maxHP; }
    }

    public int Attack {
        get { return attack; }
    }

    public int SpAttack {
        get { return spAttack; }
    }

    public int Defense {
        get { return defense; }
    }

    public int SpDefense {
        get { return spDefense; }
    }

    public int Speed {
        get { return speed; }
    }
}

public enum BubblemonType 
{
    none,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}

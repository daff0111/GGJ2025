using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Bubblemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] BubblemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    public string Name {
        get { return name;}
    }

    public string Description {
        get { return description;}
    }

    public BubblemonType Type {
        get { return type;}
    }

    public int Power {
        get { return power;}
    }

    public int Accuracy {
        get { return accuracy;}
    }

    public int PP {
        get { return pp;}
    }

    public bool IsSpecial {
        get {
            if (type == BubblemonType.Fire || type == BubblemonType.Water || type == BubblemonType.Grass
                || type == BubblemonType.Ice || type == BubblemonType.Electric || type == BubblemonType.Dragon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

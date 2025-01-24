using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }
    public int pp { get; set; }

    public Move(MoveBase pBase, int pp)
    {
        Base = pBase;
        pp = pp;
    }
}

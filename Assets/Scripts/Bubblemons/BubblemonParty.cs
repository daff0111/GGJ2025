using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubblemonParty : MonoBehaviour
{
    [SerializeField] List<Bubblemon> bubblemons;

    public List<Bubblemon> Bubblemons {
        get {
            return bubblemons;
        }
    }

    private void Start()
    {
        foreach (var bubblemon in bubblemons)
        {
            bubblemon.Init();
        }
    }

    public Bubblemon GetHealthyBubblemon()
    {
        return bubblemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}

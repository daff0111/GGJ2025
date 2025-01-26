using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Bubblemon> wildBubblemons;

    public Bubblemon GetRandomWildBubblemon()
    {
        var wildBubblemon = wildBubblemons[Random.Range(0, wildBubblemons.Count)];
        wildBubblemon.Init();
        return wildBubblemon;
    }
}

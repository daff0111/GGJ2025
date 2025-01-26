using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PuzzleFountain : MonoBehaviour
{
    public bool isRepaired = false;
    public Luminia aquorScript;
    public Dialog interactedDialog;
    public Dialog repairedDialog;
    public GameObject darkShard;

    private NPCController myNPCController;
    private Animator myAnimator;
    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myNPCController = GetComponent<NPCController>();
        myNPCController.OnInteractStarted += InteractStarted;
        myNPCController.OnInteractEnded += InteractEnded;

        myAnimator = GetComponent<Animator>();
        myAnimator.enabled = isRepaired;

        myAudioSource = GetComponent<AudioSource>();
    }

    void InteractStarted()
    {
        if(!isRepaired && aquorScript != null && aquorScript.recruited)
        {
            myNPCController.NPCDialog = interactedDialog;
            isRepaired = true;
        }
    }

    void InteractEnded()
    {
        if (isRepaired && !myAnimator.enabled)
        {
            myAnimator.enabled = true;
            myNPCController.NPCDialog = repairedDialog;
            if(myAudioSource != null)
            {
                myAudioSource.Play();
            }
            darkShard.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

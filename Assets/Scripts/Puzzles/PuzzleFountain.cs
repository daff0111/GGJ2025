using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Animations;
using UnityEngine;

public class PuzzleFountain : MonoBehaviour
{
    public bool isRepaired = false;
    public Luminia aquorScript;
    public Dialog interactedDialog;
    public Dialog repairedDialog;

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
        if(aquorScript != null && aquorScript.recruited)
        {
            myNPCController.NPCDialog = interactedDialog;
            isRepaired = true;
        }
    }

    void InteractEnded()
    {
        if (isRepaired)
        {
            myAnimator.enabled = true;
            myNPCController.NPCDialog = interactedDialog;
            if(myAudioSource != null)
            {
                myAudioSource.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

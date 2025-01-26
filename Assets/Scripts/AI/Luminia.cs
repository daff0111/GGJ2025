using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luminia : MonoBehaviour
{
    public bool recruited = false;
    public NPCController guideNPC;
    [SerializeField] public Dialog NPCRecruitedDialog;

    private NPCController nPCController;
    private CompanionController companionController;

    // Start is called before the first frame update
    void Start()
    {
        nPCController = GetComponent<NPCController>();
        companionController = GetComponent<CompanionController>();
        if(nPCController != null )
        {
            nPCController.OnInteractEnded += OnInteracted;
        }
    }

    void OnInteracted()
    {
        nPCController.enabled = false;
        companionController.enabled = true;
        nPCController.OnInteractEnded -= OnInteracted;
        recruited = true;
        if(guideNPC != null)
        {
            guideNPC.NPCDialog = NPCRecruitedDialog;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

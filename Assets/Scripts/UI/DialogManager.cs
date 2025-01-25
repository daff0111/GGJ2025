using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;
    public AudioClip confirmSound;
    private bool isActive = false;
    private bool canAdvance = false;
    private int lineIndex = 0;
    private Dialog activeDialog;

    public event Action OnDialogStarted;
    public event Action OnDialogEnded;

    private AudioSource audioSource;

    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        isActive = false;
        dialogBox.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
    public void HandleUpdate()
    {
        if (isActive)
        {
            if(canAdvance && (MobileControls.Manager.GetMobileButtonDown("ButtonA") || Input.GetKeyDown(KeyCode.Z)))
            {
                lineIndex++;
                if (lineIndex < activeDialog.Lines.Count)
                {
                    PlayConfirmSound();
                    StartCoroutine(TypeDialog(activeDialog.Lines[lineIndex]));
                }
                else
                {
                    HideDialog();
                }
            }
            else if(MobileControls.Manager.GetMobileButtonDown("ButtonB") || Input.GetKeyDown(KeyCode.B))
            {
                HideDialog();
            }
        }
    }

    // Start is called before the first frame update
    public void ShowDialog(Dialog dialog)
    {
        isActive = true;
        lineIndex = 0;
        activeDialog = dialog;
        dialogBox.SetActive(true);
        OnDialogStarted.Invoke();

        PlayConfirmSound();
        StartCoroutine(TypeDialog(dialog.Lines[lineIndex]));
    }

    public void HideDialog()
    {
        OnDialogEnded.Invoke();
        dialogBox.SetActive(false);
        
        isActive = false;
    }

    public void PlayConfirmSound()
    {
        if (audioSource != null)
        {
            audioSource.clip = confirmSound;
            audioSource.Play();
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        canAdvance = false;
        dialogText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1.0f / lettersPerSecond);
        }

        canAdvance = true;
    }
}

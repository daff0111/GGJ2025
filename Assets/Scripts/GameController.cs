using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] DialogManager dialogManager;
    [SerializeField] Camera worldCamera;
    GameState state;

    public Action OnBattleEnded;

    public static GameController Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        dialogManager.OnDialogStarted += StartDialog;
        dialogManager.OnDialogEnded += EndDialog;
    }

    void StartBattle()
    {
        var wildBubblemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildBubblemon();

        StartBattle(wildBubblemon);
    }

    public void StartBattle(Bubblemon enemy)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<BubblemonParty>();

        battleSystem.StartBattle(playerParty, enemy);
    }

    public void PlayMusic(AudioClip battleAudio)
    {
        battleSystem.GetComponent<AudioSource>().clip = battleAudio;
        battleSystem.GetComponent<AudioSource>().Play();
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        OnBattleEnded?.Invoke(); 
    }

    void StartDialog()
    {
        state = GameState.Dialog;
        battleSystem.gameObject.SetActive(false);
    }

    void EndDialog()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            dialogManager.HandleUpdate();
        }
    }
}

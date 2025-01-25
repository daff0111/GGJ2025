using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;
    private Vector2 playerInput;

    BattleState state;
    int currentAction;
    int currentMove;

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Bubblemon);
        enemyHud.SetData(enemyUnit.Bubblemon);

        dialogBox.SetMoveNames(playerUnit.Bubblemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Bubblemon.Base.Name} appeared.");

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        print("Party Screen");
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    void PlayerRun()
    {
        state = BattleState.Busy;
        StartCoroutine(HandleRunAttempt());
    }

    IEnumerator HandleRunAttempt()
    {
        yield return dialogBox.TypeDialog("You ran away safely!");
        
        // Finalizar la batalla
        OnBattleOver?.Invoke(false);

        // Opcional: Desactivar el sistema de batalla
        gameObject.SetActive(false);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Bubblemon.Moves[currentMove];
        move.PP--;
        yield return dialogBox.TypeDialog($"{playerUnit.Bubblemon.Base.Name} used {move.Base.Name}");

        playerUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyUnit.PlayHitAnimation();
        var damageDetails = enemyUnit.Bubblemon.TakeDamage(move, playerUnit.Bubblemon);
        yield return enemyHud.UpdateHP();

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Bubblemon.Base.Name} Fainted");
            enemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.Bubblemon.GetRandomMove();
        move.PP--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Bubblemon.Base.Name} used {move.Base.Name}");

        enemyUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerUnit.PlayHitAnimation();
        var damageDetails = playerUnit.Bubblemon.TakeDamage(move, playerUnit.Bubblemon);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Bubblemon.Base.Name} Fainted");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialogBox.TypeDialog("A critical hit!");

        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("It’s super effective!");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("It’s not very effective...");
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if ((MobileControls.Manager.GetMobileButtonDown("ButtonA") || Input.GetKeyDown(KeyCode.Z)))
        {
            if (currentAction == 0)
            {
                // Fight
                PlayerMove();
            }
            else if (currentAction == 1)
            {
                // Bag
            }
            else if (currentAction == 2)
            {
                // Bubblemon
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                // Run
                PlayerRun();
            }
        }
    }

    void HandleMoveSelection()
    {
        if (MobileControls.Manager.GetJoystickRight("Joystick") || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Bubblemon.Moves.Count - 1)
                ++currentMove;
        }
        else if (MobileControls.Manager.GetJoystickLeft("Joystick") || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                --currentMove;
        }
        else if (MobileControls.Manager.GetJoystickDown("Joystick") || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Bubblemon.Moves.Count - 2)
                currentMove += 2;
        }
        else if (MobileControls.Manager.GetJoystickUp("Joystick") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Bubblemon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Bubblemon.Moves[currentMove]);

        if ((MobileControls.Manager.GetMobileButtonDown("ButtonA") || Input.GetKeyDown(KeyCode.Z)))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        if ((MobileControls.Manager.GetMobileButtonDown("ButtonB") || Input.GetKeyDown(KeyCode.B)))
        {
            dialogBox.EnableMoveSelector(false);
            PlayerAction();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }
}

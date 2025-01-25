using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    public event Action OnEncountered;
    public LayerMask interactLayer;
    public float interactDelay = 1.0f;

    private bool isMoving;
    private bool isInteracting;
    private Vector2 input;
    private Vector2 lastDirection = new Vector2(0, -1);
    private Interactable interactedObject;
    private float interactTimer;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isInteracting = false;
    }

    public void HandleUpdate()
    {
        if (!isInteracting)
        {
            if (interactTimer <= 0 && (MobileControls.Manager.GetMobileButtonDown("ButtonA") || Input.GetKeyDown(KeyCode.Z)))
            {
                Interact();
            }
            if (interactTimer > 0)
                interactTimer -= Time.deltaTime;
        }
        else
        {
            if (MobileControls.Manager.GetMobileButton("ButtonB") || Input.GetKeyDown(KeyCode.B))
            {
                StopInteract();
            }
        }

        if (!isInteracting && !isMoving)
        {
            // Reset del input al inicio del frame
            input = Vector2.zero;

            if (MobileControls.Manager != null)
                input = MobileControls.Manager.GetJoystick("Joystick");

            // Si el Joystick no est� presente usar el input por defecto
            if (input == Vector2.zero)
            {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");
            }

            input.Normalize();

            // Remover movimiento diagonal
            if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
                input.y = 0;
            else
                input.x = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                lastDirection = input;

                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();
    }

    private void Interact()
    {
        Vector3 interactPosition = transform.position + new Vector3(lastDirection.x, lastDirection.y);
        Collider2D collider = Physics2D.OverlapCircle(interactPosition, 0.3f, interactLayer);
        if (collider != null)
        {
            Debug.Log("Interactuando con " + collider.gameObject.name);

            isMoving = false;
            isInteracting = true;
            animator.SetBool("isMoving", false);

            interactedObject = collider.gameObject.GetComponent<Interactable>();
            if (interactedObject != null)
            {
                interactedObject.Interact(this.gameObject);
                DialogManager.Instance.OnDialogEnded += OnDialogEnded;
            }
        }
    }

    private void StopInteract()
    {
        if (isInteracting)
        {
            isInteracting = false;
            DialogManager.Instance.OnDialogEnded -= OnDialogEnded;
            interactTimer = interactDelay;
            if (interactedObject != null)
            {
                interactedObject.StopInteract();
            }
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer);
        if (collider != null && collider.gameObject != this.gameObject && collider.GetComponent<CompanionController>() == null)
        {
            return false;
        }

        return true;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }
    void OnDialogEnded()
    {
        StopInteract();
    }
}
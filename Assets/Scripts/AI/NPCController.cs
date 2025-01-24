using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public float moveInterval = 2f; // Tiempo entre movimientos del NPC
    public List<Vector2> pathSteps; // Pasos determinados para un Loop


    private bool isMoving;
    private Vector2 moveDirection;
    private int stepIndex = 0;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(MoveNPC());
    }

    IEnumerator MoveNPC()
    {
        while (true)
        {
            // Intervalo de pausa
            yield return new WaitForSeconds(moveInterval);

            // Si no te estas moviendo calcula una nueva direccion
            if (!isMoving)
            {
                DecideMoveDirection();

                var targetPos = transform.position;
                targetPos.x += moveDirection.x;
                targetPos.y += moveDirection.y;
                Vector3 moveStep = moveDirection.normalized;

                // Chequea si no hay obstaculos
                if (moveDirection != Vector2.zero && IsWalkable(targetPos) && IsWalkable(transform.position + moveStep))
                {
                    // Mover al Animator
                    animator.SetFloat("moveX", moveDirection.x);
                    animator.SetFloat("moveY", moveDirection.y);
                    animator.SetBool("isMoving", true);

                    yield return StartCoroutine(Move(targetPos));

                    if (pathSteps.Count > 0)
                    {
                        stepIndex = (stepIndex + 1) % pathSteps.Count;
                    }
                }
                else
                {
                    // Frenar si no hay movimiento
                    animator.SetBool("isMoving", false);
                }
            }
        }
    }

    private void DecideMoveDirection()
    {
        //Seguir secuencia de pasos determinados (Loop)
        if (pathSteps.Count > 0)
        {
            moveDirection = pathSteps[stepIndex];
            return;
        }

        // O escoge una direccion random
        int randomDir = Random.Range(0, 5); // 0: stay, 1: up, 2: down, 3: left, 4: right
        switch (randomDir)
        {
            case 0:
                moveDirection = Vector2.zero; // Stay
                break;
            case 1:
                moveDirection = Vector2.up;
                break;
            case 2:
                moveDirection = Vector2.down;
                break;
            case 3:
                moveDirection = Vector2.left;
                break;
            case 4:
                moveDirection = Vector2.right;
                break;
        }
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
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, 0.25f, solidObjectsLayer);
        if (collider != null && collider.gameObject != this.gameObject)
        {
            return false;
        }
        return true;
    }
}
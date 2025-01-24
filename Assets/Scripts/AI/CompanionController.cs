using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float moveThreshold = 0.1f;
    public LayerMask solidObjectsLayer;
    public GameObject followTargetObject;
    public Vector2 followOffset;

    private bool isMoving;
    private Vector3 moveDirection;
    private int stepIndex = 0;
    private PlayerController followTargetPlayer;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(followTargetObject != null )
            followTargetPlayer = followTargetObject.GetComponent<PlayerController>();
    }

    private void Start()
    {
        StartCoroutine(MoveCompanion());
    }

    IEnumerator MoveCompanion()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            // Si no te estas moviendo calcula si el target se ha movido
            if (!isMoving)
            {
                Vector2 targetPos = GetMoveTarget();

                if (Vector2.Distance(targetPos, transform.position) > moveThreshold)
                {
                    Vector2 currentPos = transform.position;
                    moveDirection = targetPos - currentPos;
                    moveDirection.Normalize();

                    // Chequea si no hay obstaculos
                    if (moveDirection != Vector3.zero && IsWalkable(targetPos) && IsWalkable(transform.position + moveDirection))
                    {
                        // Mover al Animator
                        animator.SetFloat("moveX", moveDirection.x);
                        animator.SetFloat("moveY", moveDirection.y);
                        animator.SetBool("isMoving", true);

                        yield return StartCoroutine(Move(targetPos));
                    }
                    else
                    {
                        // Frenar si no hay movimiento
                        animator.SetBool("isMoving", false);
                    }
                }
            }
        }
    }

    private Vector2 GetMoveTarget()
    {
        Vector2 moveTarget = transform.position;

        if(followTargetObject != null)
        {
            Vector2 currentOffset = followOffset;
            if (followTargetPlayer)
            {
                //Rotar el offset segun la direccion del jugador
                Vector2 targetDirection = followTargetPlayer.GetLastDirection();
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x);
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                float rotatedX = cos * currentOffset.x - sin * currentOffset.y;
                float rotatedY = sin * currentOffset.x + cos * currentOffset.y;

                currentOffset = new Vector2(rotatedX, rotatedY);
            }

            moveTarget = new Vector2(followTargetObject.transform.position.x, followTargetObject.transform.position.y) + currentOffset;
        }

        return moveTarget;
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            targetPos = GetMoveTarget();
            float speedModifier = 1;
            if ((targetPos - transform.position).sqrMagnitude > moveSpeed)
                speedModifier = 2;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime * speedModifier);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, 0.25f, solidObjectsLayer);
        if (collider != null && collider.gameObject != followTargetObject 
            && collider.gameObject != this.gameObject && collider.GetComponent<CompanionController>() == null)
        {
            return false;
        }
        return true;
    }
}
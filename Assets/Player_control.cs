using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class c : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;

    Animator animator;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool moveSuccess = TryMove();

            if (moveSuccess)
            {
                int count = rb.Cast(
                    movementInput,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset
                );

                if (count <= 0)
                {
                    rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
                }
            }

            animator.SetBool("isWalk", moveSuccess);
            animator.SetFloat("DirectionX", movementInput.x);
            animator.SetFloat("DirectionY", movementInput.y);

            castCollisions.Clear(); // Clear the list after usage
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    bool TryMove()
    {
        return movementInput.magnitude > 0.1f;
    }
}


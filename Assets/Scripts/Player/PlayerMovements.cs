using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 6f;
    public SpriteRenderer playerSprite;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Animator animator;
    private string Up = "GoingUp";
    private string Down = "GoingDown";
    private string Walk = "IsWalking";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;

        // 1. Gestion du Flip (Gauche / Droite)
        if (moveX < 0)
        {
            playerSprite.flipX = true;
        }
        else if (moveX > 0)
        {
            playerSprite.flipX = false;
        }

        // 2. Priorité des animations selon la touche pressée
        bool isMovingVertical = Mathf.Abs(moveY) > 0.1f;
        bool isMovingHorizontal = Mathf.Abs(moveX) > 0.1f;

        // Vers le HAUT
        if (moveY > 0)
        {
            animator.SetBool(Up, true);
            animator.SetBool(Down, false);
            animator.SetBool(Walk, false);
        }
        // Vers le BAS
        else if (moveY < 0)
        {
            animator.SetBool(Up, false);
            animator.SetBool(Down, true);
            animator.SetBool(Walk, false);
        }
        // Gauche ou Droite
        else if (isMovingHorizontal)
        {
            animator.SetBool(Up, false);
            animator.SetBool(Down, false);
            animator.SetBool(Walk, true);
        }
        // Immobile (Idle)
        else
        {
            animator.SetBool(Up, false);
            animator.SetBool(Down, false);
            animator.SetBool(Walk, false);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Composants")]
    public SpriteRenderer playerSprite;
    private Animator animator;
    private Rigidbody2D rb;

    private Vector2 movementInput;

    private string Up = "GoingUp";
    private string Down = "GoingDown";
    private string Walk = "IsWalking";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;

        // Flip Sprite (Gauche / Droite)
        if (moveX < 0)
        {
            playerSprite.flipX = true;
        }
        else if (moveX > 0)
        {
            playerSprite.flipX = false;
        }

        // Gestion des états d'animation en direct
        if (moveY > 0)
        {
            SetAnimState(up: true, down: false, walk: false);
        }
        else if (moveY < 0)
        {
            SetAnimState(up: false, down: true, walk: false);
        }
        else if (moveX != 0)
        {
            SetAnimState(up: false, down: false, walk: true);
        }
        else
        {
            SetAnimState(up: false, down: false, walk: false);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void SetAnimState(bool up, bool down, bool walk)
    {
        if (animator == null) return;

        animator.SetBool(Up, up);
        animator.SetBool(Down, down);
        animator.SetBool(Walk, walk);
    }
}
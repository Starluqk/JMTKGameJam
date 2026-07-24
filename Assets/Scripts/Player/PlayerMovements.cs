using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [Header("D�placement")]
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

        bool pressUp = Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool pressDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool pressLeft = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool pressRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (pressLeft)
        {
            playerSprite.flipX = true;
        }
        else if (pressRight)
        {
            playerSprite.flipX = false;
        }

        if (pressUp)
        {
            animator.SetBool(Up, true);
            animator.SetBool(Down, false);
            animator.SetBool(Walk, false);
        }
        else if (pressDown)
        {
            animator.SetBool(Up, false);
            animator.SetBool(Down, true);
            animator.SetBool(Walk, false);
        }
        else if (pressLeft || pressRight)
        {
            animator.SetBool(Up, false);
            animator.SetBool(Down, false);
            animator.SetBool(Walk, true);
        }
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
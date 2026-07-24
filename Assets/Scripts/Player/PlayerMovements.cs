using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 6f;

    [Tooltip("Plus la valeur est élevée, moins le personnage glisse.")]
    [SerializeField] private float accelerationTime = 0.08f;

    [Tooltip("Temps nécessaire pour s'arrêter complètement.")]
    [SerializeField] private float decelerationTime = 0.18f;

    [Header("Composants")]
    public SpriteRenderer playerSprite;

    private Animator animator;
    private Rigidbody2D rb;

    private Vector2 movementInput;
    private Vector2 currentVelocity;
    private Vector2 velocityRef;

    private readonly string Up = "GoingUp";
    private readonly string Down = "GoingDown";
    private readonly string Walk = "IsWalking";

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

        // Retourner le sprite
        if (moveX < 0)
        {
            playerSprite.flipX = true;
        }
        else if (moveX > 0)
        {
            playerSprite.flipX = false;
        }

        // Animations
        if (moveY > 0)
        {
            SetAnimState(up: true, down: false, walk: false);
        }
        else if (moveY < 0)
        {
            SetAnimState(up: false, down: true, walk: false);
        }
        else if (Mathf.Abs(currentVelocity.x) > 0.05f)
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
        Vector2 targetVelocity = movementInput * moveSpeed;

        float smoothTime = movementInput.sqrMagnitude > 0f
            ? accelerationTime
            : decelerationTime;

        currentVelocity = Vector2.SmoothDamp(
            currentVelocity,
            targetVelocity,
            ref velocityRef,
            smoothTime);

        rb.linearVelocity = currentVelocity;
    }

    private void SetAnimState(bool up, bool down, bool walk)
    {
        if (animator == null) return;

        animator.SetBool(Up, up);
        animator.SetBool(Down, down);
        animator.SetBool(Walk, walk);
    }
}

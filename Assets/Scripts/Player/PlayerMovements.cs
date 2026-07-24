using UnityEngine;
using UnityEngine.VFX;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 6f;

    [Tooltip("Plus la valeur est élevée, moins le personnage glisse.")]
    [SerializeField] private float accelerationTime = 0.08f;

    [Tooltip("Temps nécessaire pour s'arrêter complètement.")]
    [SerializeField] private float decelerationTime = 0.18f;

    [Header("VFX Graph")]
    [Tooltip("Le composant VisualEffect de ton VFX Graph")]
    [SerializeField] private VisualEffect moveVFX;

    [Tooltip("Nom de l'événement d'arrêt dans le VFX Graph (par défaut 'OnStop')")]
    [SerializeField] private string stopEventName = "OnStop";

    [Tooltip("Nom de l'événement de démarrage dans le VFX Graph (par défaut 'OnPlay')")]
    [SerializeField] private string playEventName = "OnPlay";

    [Header("Composants")]
    public SpriteRenderer playerSprite;

    private Animator animator;
    private Rigidbody2D rb;

    private Vector2 movementInput;
    private Vector2 currentVelocity;
    private Vector2 velocityRef;

    private bool isVFXPlaying = false;

    private readonly string Up = "GoingUp";
    private readonly string Down = "GoingDown";
    private readonly string Walk = "IsWalking";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();

        if (moveVFX != null)
        {
            moveVFX.SendEvent(stopEventName);
        }
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;

        if (moveX < 0)
        {
            playerSprite.flipX = true;
        }
        else if (moveX > 0)
        {
            playerSprite.flipX = false;
        }

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

        HandleVFXGraph();
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

    private void HandleVFXGraph()
    {
        if (moveVFX == null) return;

        bool isMoving = currentVelocity.magnitude > 0.1f;

        if (isMoving && !isVFXPlaying)
        {
            moveVFX.SendEvent(playEventName);
            isVFXPlaying = true;
        }
        else if (!isMoving && isVFXPlaying)
        {
            moveVFX.SendEvent(stopEventName);
            isVFXPlaying = false;
        }
    }

    private void SetAnimState(bool up, bool down, bool walk)
    {
        if (animator == null) return;

        animator.SetBool(Up, up);
        animator.SetBool(Down, down);
        animator.SetBool(Walk, walk);
    }
}
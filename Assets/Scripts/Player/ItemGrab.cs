using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [Header("D�tection")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float grabRadius = 1.5f;
    [SerializeField] private float holdDistance = 1.5f;
    public bool chickenIsGrabbed = false;
    public bool extinctorIsGrabbed = false;

    [Header("Physique d'Attraction & Lancer")]
    [Range(0.01f, 1f)]
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float defaultItemDamping = 10f;
    [SerializeField] private float throwForce = 12f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string isCarryingBool = "IsCarrying";
    [SerializeField] private string releaseTrigger = "Release";
    [SerializeField] private string throwTrigger = "Throw";

    private Rigidbody2D grabbedRb;
    private Vector2 currentVelocity = Vector2.zero;
    private Camera mainCamera;
    private Vector2 grabDirection = Vector2.up;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateHoldPointPosition();

        if (Input.GetMouseButtonDown(0))
        {
            TryGrabItem();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseItem();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowItem();
        }
    }

    private void UpdateHoldPointPosition()
    {
        if (mainCamera == null || holdPoint == null) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized;

        if (direction != Vector2.zero)
        {
            grabDirection = direction;
            holdPoint.position = (Vector2)transform.position + grabDirection * holdDistance;
        }
    }

    private void FixedUpdate()
    {
        if (grabbedRb == null)
        {
            currentVelocity = Vector2.zero;

            if (animator != null && animator.GetBool(isCarryingBool))
            {
                animator.SetBool(isCarryingBool, false);
                animator.SetTrigger(releaseTrigger);
            }
            return;
        }

        Vector2 targetPosition = holdPoint.position;
        Vector2 newPosition = Vector2.SmoothDamp(grabbedRb.position, targetPosition, ref currentVelocity, smoothTime);
        grabbedRb.MovePosition(newPosition);
    }

    private void TryGrabItem()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(holdPoint.position, grabRadius);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("item"))
            {
                if (collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    grabbedRb = rb;
                    grabbedRb.linearDamping = defaultItemDamping;

                    chickenIsGrabbed = (collider.gameObject.layer == LayerMask.NameToLayer("Chicken"));
                    extinctorIsGrabbed = (collider.gameObject.layer == LayerMask.NameToLayer("Extinctor"));

                    if (animator != null)
                    {
                        animator.SetBool(isCarryingBool, true);
                    }
                    return;
                }
            }
        }
    }

    private void ReleaseItem()
    {
        if (grabbedRb != null)
        {
            grabbedRb.linearDamping = defaultItemDamping;
            grabbedRb = null;

            chickenIsGrabbed = false;
            extinctorIsGrabbed = false;

            if (animator != null)
            {
                animator.SetBool(isCarryingBool, false);
                animator.SetTrigger(releaseTrigger);
            }
        }
    }

    private void ThrowItem()
    {
        if (grabbedRb != null)
        {
            Rigidbody2D rbToThrow = grabbedRb;

            grabbedRb.linearDamping = defaultItemDamping;
            grabbedRb = null;

            chickenIsGrabbed = false;

            rbToThrow.AddForce(grabDirection * throwForce, ForceMode2D.Impulse);
            chickenIsGrabbed = false;

            if (animator != null)
            {
                animator.SetBool(isCarryingBool, false);

                if (!string.IsNullOrEmpty(throwTrigger))
                {
                    animator.SetTrigger(throwTrigger);
                }
                else
                {
                    animator.SetTrigger(releaseTrigger);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (holdPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(holdPoint.position, grabRadius);
        }
    }
}
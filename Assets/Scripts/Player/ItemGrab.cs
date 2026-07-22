using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [Header("Détection")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float grabRadius = 1.5f;
    [SerializeField] private LayerMask itemLayer;

    [Header("Physique d'Attraction")]
    [Range(0.01f, 1f)]
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string isCarryingBool = "IsCarrying";
    [SerializeField] private string releaseTrigger = "Release";

    private Rigidbody2D grabbedRb;
    private Vector2 currentVelocity = Vector2.zero;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabItem();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseItem();
        }
    }

    private void FixedUpdate()
    {
        if (grabbedRb != null)
        {
            Vector2 targetPosition = holdPoint.position;
            Vector2 newPosition = Vector2.SmoothDamp(grabbedRb.position, targetPosition, ref currentVelocity, smoothTime);
            grabbedRb.MovePosition(newPosition);
        }
    }

    private void TryGrabItem()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(holdPoint.position, grabRadius, itemLayer);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("item"))
            {
                if (collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    grabbedRb = rb;
                    grabbedRb.linearDamping = 5f;

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
            grabbedRb.linearDamping = 0f;
            grabbedRb = null;

            if (animator != null)
            {
                animator.SetBool(isCarryingBool, false);
                animator.SetTrigger(releaseTrigger);
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
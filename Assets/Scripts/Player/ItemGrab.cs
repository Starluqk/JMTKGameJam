using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [Header("Détection")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float grabRadius = 1.5f;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowItem();
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

            Vector2 throwDirection = transform.right;

            rbToThrow.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

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
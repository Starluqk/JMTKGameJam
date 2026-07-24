using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [Header("Détection")]
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

    [Header("Bras Visuels (Sprites Rectangles)")]
    [SerializeField] private Transform leftArmTransform;
    [SerializeField] private Transform rightArmTransform;
    [SerializeField] private Vector2 leftArmOffset = new Vector2(-0.025f, 0f);
    [SerializeField] private Vector2 rightArmOffset = new Vector2(0.025f, 0f);
    [SerializeField] private float armYOffset = 0.03f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string isCarryingBool = "IsCarrying";
    [SerializeField] private string releaseTrigger = "Release";
    [SerializeField] private string throwTrigger = "Throw";

    private Rigidbody2D grabbedRb;
    private Collider2D grabbedCollider; // Variable ajoutée pour mémoriser le Collider
    private Vector2 currentVelocity = Vector2.zero;
    private Camera mainCamera;
    private Vector2 grabDirection = Vector2.up;

    private SpriteRenderer leftArmSprite;
    private SpriteRenderer rightArmSprite;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        mainCamera = Camera.main;

        if (leftArmTransform != null)
            leftArmSprite = leftArmTransform.GetComponent<SpriteRenderer>();

        if (rightArmTransform != null)
            rightArmSprite = rightArmTransform.GetComponent<SpriteRenderer>();

        ToggleArms(false);
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

        UpdateArmsVisuals();
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
                    grabbedCollider = collider; // Mémorise le collider de l'objet
                    grabbedCollider.isTrigger = true; // Passe en Trigger lors du grab

                    grabbedRb.linearDamping = defaultItemDamping;

                    chickenIsGrabbed = (collider.gameObject.layer == LayerMask.NameToLayer("Chicken"));
                    extinctorIsGrabbed = (collider.gameObject.layer == LayerMask.NameToLayer("Extinctor"));

                    if (animator != null)
                    {
                        animator.SetBool(isCarryingBool, true);
                    }

                    ToggleArms(true);
                    return;
                }
            }
        }
    }

    private void ReleaseItem()
    {
        if (grabbedRb != null)
        {
            ResetGrabbedItemPhysics();

            if (animator != null)
            {
                animator.SetBool(isCarryingBool, false);
                animator.SetTrigger(releaseTrigger);
            }

            ToggleArms(false);
        }
    }

    private void ThrowItem()
    {
        if (grabbedRb != null)
        {
            Rigidbody2D rbToThrow = grabbedRb;

            ResetGrabbedItemPhysics();

            rbToThrow.AddForce(grabDirection * throwForce, ForceMode2D.Impulse);

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

            ToggleArms(false);
        }
    }

    /// <summary>
    /// Remet le collider en non-trigger et réinitialise les références de l'objet porté.
    /// </summary>
    private void ResetGrabbedItemPhysics()
    {
        if (grabbedCollider != null)
        {
            grabbedCollider.isTrigger = false; // Repasse le collider en normal
            grabbedCollider = null;
        }

        if (grabbedRb != null)
        {
            grabbedRb.linearDamping = defaultItemDamping;
            grabbedRb = null;
        }

        chickenIsGrabbed = false;
        extinctorIsGrabbed = false;
    }

    #region Gestion Visuelle des Bras

    private void UpdateArmsVisuals()
    {
        if (grabbedRb == null)
        {
            ToggleArms(false);
            return;
        }

        ToggleArms(true);
        AnchorArmToTarget(leftArmTransform, leftArmOffset);
        AnchorArmToTarget(rightArmTransform, rightArmOffset);
    }

    private void AnchorArmToTarget(Transform arm, Vector2 shoulderOffset)
    {
        if (arm == null || grabbedRb == null) return;

        arm.localPosition = new Vector2(shoulderOffset.x, shoulderOffset.y + armYOffset);

        Vector3 shoulderWorldPos = arm.position;
        Vector3 itemWorldPos = grabbedRb.position;
        Vector2 direction = itemWorldPos - shoulderWorldPos;
        float distance = direction.magnitude;

        if (distance < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arm.rotation = Quaternion.Euler(0f, 0f, angle + 90f);

        arm.localScale = new Vector3(arm.localScale.x, distance, arm.localScale.z);
    }

    private void ToggleArms(bool state)
    {
        if (leftArmSprite != null) leftArmSprite.enabled = state;
        if (rightArmSprite != null) rightArmSprite.enabled = state;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        if (holdPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(holdPoint.position, grabRadius);
        }
    }
}
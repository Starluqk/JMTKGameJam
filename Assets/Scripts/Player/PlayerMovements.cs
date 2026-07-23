using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 movementInput;
    private Vector2 mouseWorldPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movementInput = new Vector2(moveX, moveY).normalized;

        if (mainCamera != null)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;

        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector2 lookDirection = mouseWorldPosition - rb.position;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            rb.MoveRotation(angle);
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Récupère la caméra principale pour convertir la position de la souris
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 1. Lecture des entrées clavier
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;

        // 2. Orientation vers la souris
        RotateTowardsMouse();
    }

    private void FixedUpdate()
    {
        // Applique le déplacement sans toucher à la rotation
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void RotateTowardsMouse()
    {
        // Récupère la position de la souris à l'écran et la convertit en coordonnées World
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // Calcule la direction entre le joueur et la souris
        Vector2 lookDirection = (Vector2)mouseWorldPosition - rb.position;

        // Calcule l'angle en degrés
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // Applique la rotation sur l'axe Z
        rb.rotation = angle;
    }
}
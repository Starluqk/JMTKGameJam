using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Cible & Décalage")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Inertie / Adhérence")]
    [Range(0.01f, 1f)]
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 currentVelocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
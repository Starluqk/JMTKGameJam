using UnityEngine;

public class WindowZone : MonoBehaviour
{
    [Header("Configuration")]

    [SerializeField] private LayerMask windowLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndDestroyItem(other.gameObject);
    }

    private void CheckAndDestroyItem(GameObject obj)
    {
        if ((windowLayer.value & (1 << obj.layer)) != 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(200);
            }

            Destroy(obj);
        }
    }
}
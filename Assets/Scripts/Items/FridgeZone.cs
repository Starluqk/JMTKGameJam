using UnityEngine;

public class FridgeZone : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Sélectionnez le ou les layers qui doivent être détruits par la poubelle (ex: Fridge).")]
    [SerializeField] private LayerMask fridgeLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndDestroyItem(other.gameObject);
    }

    private void CheckAndDestroyItem(GameObject obj)
    {
        if ((fridgeLayer.value & (1 << obj.layer)) != 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(100);
            }

            Destroy(obj);
        }
    }
}
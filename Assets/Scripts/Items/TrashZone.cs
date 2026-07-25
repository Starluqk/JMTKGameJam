using UnityEngine;
using UnityEngine.Audio;
public class TrashZone : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private LayerMask trashLayer;
    
    [SerializeField] private audioclass audioclass;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndDestroyItem(other.gameObject);
    }

    private void CheckAndDestroyItem(GameObject obj)
    {
        if ((trashLayer.value & (1 << obj.layer)) != 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(100);
            }
            audioclass.playClipOnce("destroy");
            Destroy(obj);
        }
    }
}
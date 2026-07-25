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
<<<<<<< HEAD
            
=======
            audioclass.playClipOnce("destroy");
>>>>>>> a700f8568f7e326325d3d1d0ffeba30169dbe2ef
            Destroy(obj);
        }
    }
}
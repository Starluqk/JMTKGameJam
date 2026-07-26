using UnityEngine;
using UnityEngine.Audio;

public class TrashZone : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private LayerMask trashLayer;
    
    [SerializeField] private AudioClip[] SoundList;
    [SerializeField] private AudioSource source;

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
            int randomNumber = Random.Range(1,3);
            source.PlayOneShot(SoundList[randomNumber]);

            Destroy(obj);
        }
    }
}
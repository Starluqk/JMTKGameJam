using UnityEngine;
using UnityEngine.Audio;

public class FridgeZone : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("S�lectionnez le ou les layers qui doivent �tre d�truits par la poubelle (ex: Fridge).")]
    [SerializeField] private LayerMask fridgeLayer;

    [SerializeField] private AudioClip[] SoundList;
    [SerializeField] private AudioSource source;

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

            int randomNumber = Random.Range(1,3);
            source.PlayOneShot(SoundList[randomNumber]);
            Destroy(obj);
        }
    }
}
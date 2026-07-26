using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToiletCleaner : MonoBehaviour
{
    [Header("Animation & Visuels")]
    [SerializeField] private Animator animator;
    [SerializeField] private string casserBoolName = "IsBroken";

    [Header("Préfab à Nettoyer")]
    [SerializeField] private GameObject dirtPrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0.105f, -0.4f, 0f);

    [Header("Réglages du Nettoyage")]
    [SerializeField] private float cleanSpeed = 0.5f;

    [Header("Identification de l'outil")]
    [SerializeField] private LayerMask broomLayer;
    [SerializeField] private ItemGrabber itemGrabber;
    [SerializeField] private audioclass audioclass;

    private float currentCleanAmount = 0f;
    private bool isFullyCleaned = true;
    private GameObject currentDirtInstance;

    private void Awake()
    {
        if (itemGrabber == null)
            itemGrabber = FindFirstObjectByType<ItemGrabber>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void SetChiotte()
    {
        if (currentDirtInstance != null)
        {
            Destroy(currentDirtInstance);
        }

        isFullyCleaned = false;
        currentCleanAmount = 0f;

        if (animator != null)
        {
            animator.SetBool(casserBoolName, true);
        }

        if (dirtPrefab != null)
        {
            Vector3 spawnPosition = transform.position + spawnOffset;
            currentDirtInstance = Instantiate(dirtPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isFullyCleaned) return;

        if ((broomLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (IsBroomGrabbed(other.gameObject))
            {
                float mouseMovement = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));

                if (mouseMovement > 0.05f)
                {
                    CleanProgress(mouseMovement * cleanSpeed * Time.deltaTime);

                    if (audioclass != null)
                    {
                        audioclass.playClipOnLoop("balais");
                    }
                }
            }
        }
    }

    private bool IsBroomGrabbed(GameObject broomObject)
    {
        if (itemGrabber == null) return false;
        return itemGrabber.GrabbedGameObject == broomObject;
    }

    private void CleanProgress(float amount)
    {
        currentCleanAmount += amount;

        if (currentCleanAmount >= 1f)
        {
            isFullyCleaned = true;

            if (animator != null)
            {
                animator.SetBool(casserBoolName, false);
            }

            if (currentDirtInstance != null)
            {
                Destroy(currentDirtInstance);
            }

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(150);
            }
        }
    }
}
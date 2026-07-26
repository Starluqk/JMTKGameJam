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

    private GameObject currentDirtInstance;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentDirtInstance == null && animator != null && animator.GetBool(casserBoolName))
        {
            animator.SetBool(casserBoolName, false);
        }
    }

    public void SetChiotte()
    {
        if (currentDirtInstance != null)
        {
            Destroy(currentDirtInstance);
        }

        if (animator != null)
        {
            animator.SetBool(casserBoolName, true);
        }

        if (dirtPrefab != null)
        {
            currentDirtInstance = Instantiate(dirtPrefab, transform.position, Quaternion.identity);

            Vector3 targetPosition = transform.position + spawnOffset;
            currentDirtInstance.transform.position = targetPosition;
        }
    }
}
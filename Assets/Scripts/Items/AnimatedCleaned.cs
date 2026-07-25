using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToiletCleaner : MonoBehaviour
{
    [Header("Animation & Nettoyage")]
    [SerializeField] private Animator animator;

    [Tooltip("Nom EXACT du State/Animation du Toilette Cassée dans l'Animator")]
    [SerializeField] private string brokenStateName = "BrokenToilet";

    [Tooltip("Nom du Trigger de transition vers Idle (ex: Cleaned)")]
    [SerializeField] private string idleTriggerName = "Cleaned";

    [Tooltip("Vitesse de nettoyage/rembobinage quand on frotte")]
    [SerializeField] private float cleanSpeed = 0.5f;

    [Header("Identification de l'outil")]
    [Tooltip("Layer du balai")]
    [SerializeField] private LayerMask broomLayer;
    [SerializeField] private ItemGrabber itemGrabber;
    [SerializeField] private audioclass audioclass;
    [SerializeField] private string Casser = "IsBroken";

    private float currentProgress = 1f;
    private bool isFullyCleaned = false;

    private void Awake()
    {
        if (itemGrabber == null)
            itemGrabber = FindFirstObjectByType<ItemGrabber>();

        if (animator == null)
            animator = GetComponent<Animator>();
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
    public void SetChiotte()
    {
        animator.SetBool(Casser, true);
    }
    private void CleanProgress(float amount)
    {
        currentProgress -= amount;
        currentProgress = Mathf.Clamp01(currentProgress);

        if (animator != null)
        {
            animator.Play(brokenStateName, 0, currentProgress);
        }

        if (currentProgress <= 0.01f)
        {
            isFullyCleaned = true;
            animator.SetBool(Casser, false);

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(150);
            }
        }
    }
}
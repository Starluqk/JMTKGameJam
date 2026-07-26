using UnityEngine;

public class EndGameSequence : MonoBehaviour
{
    [Header("Caméra & Nouvelle Cible")]
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Transform endCameraTarget;

    [Header("Animation de Fin")]
    [SerializeField] private Animator endAnimator;
    [SerializeField] private string animationTriggerName = "PlayEndAnim";

    private bool isSequenceTriggered = false;

    private void Update()
    {
        if (!isSequenceTriggered && ScoreManager.Instance != null)
        {
            if (ScoreManager.Instance.GetTimeRemaining() <= 0f)
            {
                TriggerEndSequence();
            }
        }
    }

    public void TriggerEndSequence()
    {
        if (isSequenceTriggered) return;
        isSequenceTriggered = true;

        if (cameraFollow != null && endCameraTarget != null)
        {
            cameraFollow.SetTarget(endCameraTarget);
        }

        if (endAnimator != null)
        {
            endAnimator.SetTrigger(animationTriggerName);
        }
    }
}
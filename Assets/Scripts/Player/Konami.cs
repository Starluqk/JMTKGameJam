using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KonamiCode : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip secretMusic;

    [Header("Settings")]
    public float inputExpiration = 0.5f;

    [Header("Élargissement du Personnage")]
    [SerializeField] private float scaleDelay = 3f; 
    [SerializeField] private float resizeDuration = 2f; 
    [SerializeField] private float targetScaleX = 2f; 

    private Key[] konamiOrder = {
        Key.UpArrow, Key.UpArrow,
        Key.DownArrow, Key.DownArrow,
        Key.LeftArrow, Key.RightArrow,
        Key.LeftArrow, Key.RightArrow
    };

    private int currentIndex = 0;
    private float lastInputTime;
    private bool isActivated = false; 

    void Update()
    {
        if (isActivated) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.anyKey.wasPressedThisFrame)
        {
            CheckInput(keyboard);
        }
    }

    void CheckInput(Keyboard kb)
    {
        if (Time.time - lastInputTime > inputExpiration)
        {
            currentIndex = 0;
        }

        if (kb[konamiOrder[currentIndex]].wasPressedThisFrame)
        {
            currentIndex++;
            lastInputTime = Time.time;

            if (currentIndex >= konamiOrder.Length)
            {
                ActivateSecret();
                currentIndex = 0;
            }
        }
        else
        {
            currentIndex = 0;
        }
    }

    void ActivateSecret()
    {
        isActivated = true;

        Debug.Log("<color=lime>KONAMI CODE RÉUSSI !</color>");

        if (audioSource != null && secretMusic != null)
        {
            audioSource.clip = secretMusic;
            audioSource.loop = false;
            audioSource.Play();
        }

        StartCoroutine(WidenPlayerRoutine());
    }

    private IEnumerator WidenPlayerRoutine()
    {
        yield return new WaitForSeconds(scaleDelay);

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(initialScale.x * targetScaleX, initialScale.y, initialScale.z);

        float elapsedTime = 0f;

        while (elapsedTime < resizeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / resizeDuration;

            transform.localScale = Vector3.Lerp(initialScale, targetScale, Mathf.SmoothStep(0f, 1f, t));

            yield return null;
        }

        transform.localScale = targetScale;
    }
}
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerFlashEffect : MonoBehaviour
{
    [Header("Référence")]
    public TextMeshProUGUI timerText;

    [Header("Flash")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.5f;

    [Header("Scale")]
    public float normalScale = 1.9f;
    public float flashScale = 2.1f;

    private Color normalColor;
    private int lastSecond = -1;

    private void Start()
    {
        normalColor = Color.white;
        timerText.color = normalColor;
        timerText.transform.localScale = Vector3.one * normalScale;
    }

    private void Update()
    {
        float time = ScoreManager.Instance.GetTimeRemaining();

        int seconds = Mathf.CeilToInt(time);

        if (seconds == lastSecond)
            return;

        lastSecond = seconds;

        if (seconds > 10 && seconds % 30 == 0)
        {
            StartCoroutine(Flash());
        }

        if (seconds <= 10 && seconds > 0)
        {
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        float timer = 0f;

        // Blanc -> Rouge
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float t = timer / flashDuration;

            timerText.color = Color.Lerp(normalColor, flashColor, t);
            timerText.transform.localScale = Vector3.Lerp(
                Vector3.one * normalScale,
                Vector3.one * flashScale,
                t
            );

            yield return null;
        }

        timer = 0f;

        // Rouge -> Blanc
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float t = timer / flashDuration;

            timerText.color = Color.Lerp(flashColor, normalColor, t);
            timerText.transform.localScale = Vector3.Lerp(
                Vector3.one * flashScale,
                Vector3.one * normalScale,
                t
            );

            yield return null;
        }

        timerText.color = normalColor;
        timerText.transform.localScale = Vector3.one * normalScale;
    }
}
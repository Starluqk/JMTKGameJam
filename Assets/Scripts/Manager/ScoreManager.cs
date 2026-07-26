using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI Textes & Panels")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI resultTextVictory;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject messageBegin;
    public bool isPlaying;
    private bool playOnce = false;

    [Header("Paramètres du Jeu")]
    [SerializeField] private float gameDuration = 60f;

    [Tooltip("Délai (en secondes) pendant lequel l'animation de fin se joue avant de mettre le jeu en pause et d'afficher le Canvas")]
    [SerializeField] private float endGameDelay = 2.5f;

    [Tooltip("La liste de tous les Layers d'objets à prendre en compte dans le nettoyage")]
    [SerializeField] private List<LayerMask> trashLayers = new List<LayerMask>();

    [SerializeField] private string scorePrefix = "Score : ";

    [SerializeField] private audioclass audioclass;

    private int currentScore = 0;
    private float gameDurationStay = 0;
    private float timeRemaining = 50f;
    private int maxTrashCountTracked = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

    }

    private void Update()
    {

        if (isGameOver) return;

        int currentTrashCount = CountTrashObjects();
        if (currentTrashCount > maxTrashCountTracked)
        {
            maxTrashCountTracked = currentTrashCount;
        }

        timeRemaining -= Time.deltaTime;
        gameDurationStay += Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerUI();
            EndGame();
        }
        else
        {
            UpdateTimerUI();
        }

        if (gameDurationStay >= 0.7f && playOnce == false)
        {
            isPlaying = false;
            messageBegin.SetActive(true);
            Time.timeScale = 0f;
            playOnce = true;
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        currentScore += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = scorePrefix + currentScore.ToString();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"{seconds}";
        }
    }

    private void EndGame()
    {
        isGameOver = true;
        StartCoroutine(EndGameRoutine());
    }

    private IEnumerator EndGameRoutine()
    {
        yield return new WaitForSecondsRealtime(endGameDelay);

        Time.timeScale = 0f;

        int remainingTrash = CountTrashObjects();

        if (remainingTrash > maxTrashCountTracked)
        {
            maxTrashCountTracked = remainingTrash;
        }

        int cleanedTrash = maxTrashCountTracked - remainingTrash;

        int percentageCleaned = 0;
        if (maxTrashCountTracked > 0)
        {
            percentageCleaned = Mathf.RoundToInt(((float)cleanedTrash / maxTrashCountTracked) * 100f);
            percentageCleaned = Mathf.Clamp(percentageCleaned, 0, 100);
        }
        else
        {
            percentageCleaned = 100;
        }

        bool isVictory = percentageCleaned >= 90;

        if (isVictory)
        {
            if (victoryPanel != null)
            {
                if (audioclass != null) audioclass.playClipOnce("victory");
                victoryPanel.SetActive(true);
            }
        }
        else
        {
            if (audioclass != null) audioclass.playClipOnce("loose");
            if (endPanel != null)
            {
                endPanel.SetActive(true);
            }
        }

        string resultMessage = $"TIME OUT !\n\nScore : {currentScore}\nCleared trash : {percentageCleaned}%";

        if (resultText != null)
        {
            resultText.text = resultMessage;
        }

        if (resultTextVictory != null)
        {
            resultTextVictory.text = resultMessage;
        }
    }

    private int CountTrashObjects()
    {
        int count = 0;
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (IsObjectInAnyTrashLayer(obj.layer))
            {
                count++;
            }
        }

        return count;
    }

    private bool IsObjectInAnyTrashLayer(int objectLayer)
    {
        foreach (LayerMask layerMask in trashLayers)
        {
            if ((layerMask.value & (1 << objectLayer)) != 0)
            {
                return true;
            }
        }
        return false;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void SetGameDuration(float time)
    {
        gameDuration = time;
    }

    public void StartTimer()
    {
        Time.timeScale = 1f;

        timeRemaining = gameDuration;

        maxTrashCountTracked = CountTrashObjects();

        if (endPanel != null)
            endPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();
    }

    public void BeginMessage()
    {
        messageBegin.SetActive(false);
        Time.timeScale = 1f;
        isPlaying = true;
    }
}
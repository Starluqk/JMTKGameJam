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
    [SerializeField] private GameObject endPanel;

    [Header("Paramètres du Jeu")]
    [SerializeField] private float gameDuration = 60f;

    [Tooltip("Liste de tous les Layers comptabilisés comme objets/taches à éliminer")]
    [SerializeField] private List<LayerMask> trashLayers = new List<LayerMask>();

    [SerializeField] private string scorePrefix = "Score : ";

    private int currentScore = 0;
    private float timeRemaining;
    private int initialTrashCount = 0;
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
        Time.timeScale = 1f;

        timeRemaining = gameDuration;
        initialTrashCount = CountTrashObjects();

        if (endPanel != null)
            endPanel.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;

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
            timerText.text = $"Temps : {seconds}s";
        }
    }

    private void EndGame()
    {
        isGameOver = true;

        Time.timeScale = 0f;

        int remainingTrash = CountTrashObjects();
        int cleanedTrash = initialTrashCount - remainingTrash;

        int percentageCleaned = 0;
        if (initialTrashCount > 0)
        {
            percentageCleaned = Mathf.RoundToInt(((float)cleanedTrash / initialTrashCount) * 100f);
            percentageCleaned = Mathf.Clamp(percentageCleaned, 0, 100);
        }
        else
        {
            percentageCleaned = 100;
        }

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.text = $"TEMPS ÉCOULÉ !\n\nScore : {currentScore}\nDéchets éliminés : {percentageCleaned}%";
        }

        Debug.Log($"Fin de partie ! Score : {currentScore} | Déchets nettoyés : {percentageCleaned}% ({cleanedTrash}/{initialTrashCount})");
    }

    private int CountTrashObjects()
    {
        int count = 0;

#pragma warning disable 0618
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
#pragma warning restore 0618

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
}
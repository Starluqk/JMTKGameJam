using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    [SerializeField] private float[] minTimeEvent;
    [SerializeField] private float[] maxTimeEvent;
    private float[] timeEvent;
    private int currentEvent = 0;
    [SerializeField] private GameObject[] EventSpawner;

    private ScoreManager scoreManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        timeEvent = new float[minTimeEvent.Length];
        for (int i = 0; i < minTimeEvent.Length; i++)
        {
            timeEvent[i] = Random.Range(minTimeEvent[i], maxTimeEvent[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeEvent.Length > currentEvent)
        {
            if (scoreManager.GetTimeRemaining() < timeEvent[currentEvent])
            {
                GetRandomEvent();
                currentEvent++;
            }
        }
       
    }

    private void GetRandomEvent()
    {
        if (EventSpawner.Length > 0)
        {
            Instantiate(EventSpawner[Random.Range(0, EventSpawner.Length)]);
        }
    }
}

using Unity.VisualScripting;
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

    public void GetRandomEvent()
    {
        if (EventSpawner.Length > 0)
        {
            GameObject obj = Instantiate(EventSpawner[Random.Range(0, EventSpawner.Length)]);
            if (!obj.GetComponent<ToiletEvent>().IsUnityNull())
            {
                obj.GetComponent<ToiletEvent>().SetRandomEvent(gameObject.GetComponent<RandomEvent>());
            }
        }
    }

    public void SetTimeEvent(float[] minte, float[] maxte)
    {
        minTimeEvent = minte;
        maxTimeEvent = maxte;
    }

    public void EventTime()
    {
        timeEvent = new float[minTimeEvent.Length];
        for (int i = 0; i < minTimeEvent.Length; i++)
        {
            timeEvent[i] = Random.Range(minTimeEvent[i], maxTimeEvent[i]);
        }
    }
    
}

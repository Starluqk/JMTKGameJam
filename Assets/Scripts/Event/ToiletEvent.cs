using UnityEngine;

public class ToiletEvent : MonoBehaviour
{
    private ToiletCleaner toilet;

    private RandomEvent rE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FindObjectsByType<ToiletEvent>(FindObjectsSortMode.None).Length > 1)
        {
            rE.GetRandomEvent();
            Destroy(gameObject);
        }
        toilet = FindAnyObjectByType<ToiletCleaner>();
        toilet.SetChiotte();
    }

    public void SetRandomEvent(RandomEvent randomEvent)
    {
        rE = randomEvent;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

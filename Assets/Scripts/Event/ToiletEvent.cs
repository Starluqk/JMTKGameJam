using UnityEngine;

public class ToiletEvent : MonoBehaviour
{
    private ToiletCleaner toilet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toilet = FindAnyObjectByType<ToiletCleaner>();
        toilet.SetChiotte();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

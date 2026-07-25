using UnityEngine;

public class OnTriggerObject : MonoBehaviour
{

    private GameObject _obj;
    private SpriteRenderer sr;
    private int layer = 95;
    private int objectLayer;

    private bool _hasToGetHigher;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr = _obj.GetComponent<SpriteRenderer>();
            if (_hasToGetHigher)
            {
                objectLayer = sr.sortingOrder;
                sr.sortingOrder = 90;
                other.GetComponent<SpriteRenderer>().sortingOrder = 89;
            }
            else
            {
                other.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            
            
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SpriteRenderer>().sortingOrder = layer;
            if (_hasToGetHigher)
            {
                sr.sortingOrder = objectLayer;
            }
        }
    }
    
    public void SetObj(GameObject obj, bool hasToGetHigher)
    {
        _obj = obj;
        _hasToGetHigher = hasToGetHigher;
    }
}

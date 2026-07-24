using UnityEngine;

public class OnTriggerObject : MonoBehaviour
{

    private GameObject _obj;
    private SpriteRenderer sr;
    private int layer;
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
            Debug.Log(sr.rendererPriority);
            layer = sr.sortingOrder;
            sr.sortingOrder = 99;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.sortingOrder = layer;
        }
    }
    
    public void SetObj(GameObject obj)
    {
        _obj = obj;
    }
}

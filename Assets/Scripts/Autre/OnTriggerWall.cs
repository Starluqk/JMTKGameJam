using System;
using Unity.VisualScripting;
using UnityEngine;

public class OnTriggerWall : MonoBehaviour
{
    private GameObject _wall;

    [SerializeField] private float _opacity = 0.4f;

    private float _wantedOpacity;

    private SpriteRenderer sr;
    private int layer;


    private void Update()
    {
        if (!sr.IsUnityNull())
        {
            if (sr.color.a < _wantedOpacity)
            {
                Color color = sr.color;
                color.a += 0.01f;
                sr.color = color; 
            }

            if (sr.color.a > _wantedOpacity)
            {
                Color color = sr.color;
                color.a -= 0.01f;
                sr.color = color; 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr = _wall.GetComponent<SpriteRenderer>();
            layer = sr.sortingOrder;
            _wantedOpacity = _opacity;
            sr.sortingOrder = 99;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _wantedOpacity = 1f;
            sr.sortingOrder = layer;
        }
    }

    public void SetWall(GameObject wall)
    {
        _wall = wall;
    }
}

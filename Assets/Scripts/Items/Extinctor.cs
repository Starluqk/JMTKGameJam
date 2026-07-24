using UnityEngine;
using UnityEngine.VFX;

public class Extinctor : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private GameObject _spray;
    private Vector3 direction;
    private VisualEffect _vfxSpray;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _vfxSpray = _spray.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _spray.SetActive(true);
            direction =transform.position -  _playerTransform.position;
            direction.z = 0;
            _vfxSpray.SetVector3("Spray", direction);
        }
        else
        {
            _spray.SetActive(false);
        }
    }
}

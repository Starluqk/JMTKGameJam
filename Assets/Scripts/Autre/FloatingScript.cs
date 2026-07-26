using UnityEngine;

public class FloatingScript : MonoBehaviour
{
   
    public float rotationAngle = 2f;   
    public float rotationSpeed = 1f;    

   
    public float scaleAmplitude = 0.03f; 
    public float scaleSpeed = 0.8f;     

    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void Update()
    {
       
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);

        float scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * scaleAmplitude;
        transform.localScale = baseScale * (1f + scaleOffset);
    }
}

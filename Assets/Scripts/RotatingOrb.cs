using UnityEngine;

public class RotatingOrb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
void Update()
{
    // Floating motion
    transform.localPosition += Vector3.up * Mathf.Sin(Time.time * 2f) * 0.0005f;

    // Slow rotation
    transform.Rotate(0, 30f * Time.deltaTime, 0);
}

}

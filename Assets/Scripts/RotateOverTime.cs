using UnityEngine;

public class RotateOverTime : MonoBehaviour
{

    public float rotationSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);

    }

    
}

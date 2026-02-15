using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartOrb : MonoBehaviour
{
    //    [Header("Hover Feedback")]
    public float scaleMultiplier = 1.15f;
    public float transitionSpeed = 8f;

    private Vector3 originalScale;
    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        // Floating motion
        transform.localPosition += Vector3.up * Mathf.Sin(Time.time * 2f) * 0.0005f;

        // Slow rotation
        transform.Rotate(0, 30f * Time.deltaTime, 0);

        // Hover Visual Feedback
        Vector3 targetScale = isHovered ? originalScale * scaleMultiplier : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
    }

    // Called by XR Simple Interactable
    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        isHovered = true;
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        isHovered = false;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingScript : MonoBehaviour
{
    public float pulseSpeed = 0.5f;   // Speed of the pulse effect
    public float pulseAmount = 0.1f; // Amount of scaling for the pulse

    private Vector3 originalScale;
    private bool isPulsing = false;

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
    }

    void Update()
    {
        if (isPulsing)
        {
            // Pulsing effect
            float scale = Mathf.PingPong(Time.time * pulseSpeed, pulseAmount) + 1;
            transform.localScale = originalScale * scale;
        }
        else
        {
            // Ensure the scale is reset if not pulsing
            transform.localScale = originalScale;
        }
    }

    public void StartPulsing()
    {
        isPulsing = true;
    }

    public void StopPulsing()
    {
        isPulsing = false;
        transform.localScale = originalScale; // Reset scale to original
    }
}

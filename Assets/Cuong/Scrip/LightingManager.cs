using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingManager : MonoBehaviour
{
    public Light2D globalLight;

    // Cường độ sáng theo buổi
    public float morningIntensity = 0.5f;
    public float noonIntensity = 0.9f;
    public float eveningIntensity = 0.4f;
    public float nightIntensity = 0.1f;

    public Color morningColor = new Color(1f, 0.95f, 0.8f);
    public Color noonColor = Color.white;
    public Color eveningColor = new Color(1f, 0.6f, 0.4f);
    public Color nightColor = new Color(0.1f, 0.1f, 0.3f);

    void OnEnable()
    {
        TimeManager.OnTimeChanged += UpdateLighting;
    }

    void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateLighting;
    }

    void UpdateLighting(float hour)
    {
        float intensity = 1f;
        Color color = Color.white;

        if (hour >= 6f && hour < 12f) // Sáng
        {
            float t = (hour - 6f) / 6f;
            intensity = Mathf.Lerp(morningIntensity, noonIntensity, t);
            color = Color.Lerp(morningColor, noonColor, t);
        }
        else if (hour >= 12f && hour < 18f) // Chiều
        {
            float t = (hour - 12f) / 6f;
            intensity = Mathf.Lerp(noonIntensity, eveningIntensity, t);
            color = Color.Lerp(noonColor, eveningColor, t);
        }
        else if (hour >= 18f && hour < 24f) // Tối
        {
            float t = (hour - 18f) / 6f;
            intensity = Mathf.Lerp(eveningIntensity, nightIntensity, t);
            color = Color.Lerp(eveningColor, nightColor, t);
        }
        else // Đêm đến sáng
        {
            float t = hour / 6f;
            intensity = Mathf.Lerp(nightIntensity, morningIntensity, t);
            color = Color.Lerp(nightColor, morningColor, t);
        }

        globalLight.intensity = intensity;
        globalLight.color = color;
    }
}


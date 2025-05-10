using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampAutoLight : MonoBehaviour
{
    public Light2D pointLight;

    void Start()
    {
        TimeManager.OnTimeChanged += CheckTime;
    }

    void OnDestroy()
    {
        TimeManager.OnTimeChanged -= CheckTime;
    }

    void CheckTime(float hour)
    {
        // Bật đèn nếu từ 18h đến trước 6h
        if (hour >= 18f || hour < 6f)
        {
            if (!pointLight.enabled)
                pointLight.enabled = true;
        }
        else
        {
            if (pointLight.enabled)
                pointLight.enabled = false;
        }
    }
}

using UnityEngine;

public class ClockPointer : MonoBehaviour
{
    private RectTransform rectTransform;
    private float targetAngle;
    public float rotationSpeed = 90f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        TimeManager.OnTimeChanged += OnTimeChanged;
    }

    void OnDisable()
    {
        TimeManager.OnTimeChanged -= OnTimeChanged;
    }

    void OnTimeChanged(float hour)
    {
        hour = Mathf.Clamp(hour, 0f, 24f);
        targetAngle = 270f - (hour / 24f) * 180f;
    }

    void Update()
    {
        float currentZ = rectTransform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, targetAngle, rotationSpeed * Time.deltaTime);
        rectTransform.localEulerAngles = new Vector3(0f, 0f, newZ);
    }
}

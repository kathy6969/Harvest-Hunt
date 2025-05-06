using UnityEngine;

public class ClockPointer : MonoBehaviour
{
    private float targetAngle = 270f; // Khởi tạo targetAngle tại 270 độ (dưới)
    public float rotationSpeed = 90f; // Tốc độ quay: độ/giây
    private RectTransform rectTransform;

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
        // Chuyển đổi giờ (0–24) thành góc (270–90 độ) để quay từ dưới lên trên
        targetAngle = -270f + (hour / 24f) * 180f;
    }

    void Update()
    {
        // Lấy góc hiện tại (Z) và chuyển thành giá trị phù hợp để tính toán
        float currentZ = transform.eulerAngles.z;
        if (currentZ < 0f) currentZ += 360f;

        float currentAngle = currentZ;
        float newAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        // Xoay kim theo chiều từ dưới lên trên
        rectTransform.localEulerAngles = new Vector3(0f, 0f, newAngle);
    }
}

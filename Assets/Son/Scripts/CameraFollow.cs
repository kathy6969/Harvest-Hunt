using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Đối tượng nhân vật để camera theo dõi
    public Vector3 offset;  // Khoảng cách giữa camera và nhân vật

    void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mong muốn của camera
        Vector3 desiredPosition = target.position + offset;

        // Di chuyển camera trực tiếp đến vị trí mong muốn
        transform.position = desiredPosition;

        // (Tùy chọn) Camera nhìn về phía nhân vật
        // transform.LookAt(target);
    }
}
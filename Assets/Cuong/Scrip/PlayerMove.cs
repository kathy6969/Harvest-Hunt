using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;     // tốc độ di chuyển
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Tải vị trí khi bắt đầu game
        transform.position = SaveManager.LoadPlayerPosition();
        Debug.Log("File path: " + Application.persistentDataPath);

    }

    void Update()
    {
        // Ví dụ: Lưu bằng phím L
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveManager.SavePlayerPosition(transform.position);
        }

        // Lấy input từ người chơi (WASD hoặc mũi tên)
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D hoặc ←/→
        float moveY = Input.GetAxisRaw("Vertical");   // W/S hoặc ↑ /↓

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Di chuyển nhân vật
        rb.velocity = moveDirection * moveSpeed;
    }
}

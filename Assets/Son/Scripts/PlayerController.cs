using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    [SerializeField] private float moveSpeed = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }
    }

    void Update()
    {
        // Lấy input di chuyển
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Tính tốc độ cho Animator
        float speed = movement.magnitude;
        animator.SetFloat("Speed", speed);
       
        // Ưu tiên hướng chính cho animation
        float directionX = movement.x;
        float directionY = movement.y;

        // Nếu đi chéo, ưu tiên hướng ngang
        if (movement.x != 0 && movement.y != 0)
        {
            directionY = 0; // Ưu tiên ngang (Left hoặc Right)
        }

        // Cập nhật DirectionX và DirectionY cho Animator
        if (speed > 0)
        {
            animator.SetFloat("DirectionX", directionX);
            animator.SetFloat("DirectionY", directionY);
            
        }

        // Kiểm tra nhấn Space để đào (chỉ khi đứng yên)
        if (Input.GetKeyDown(KeyCode.Space) && speed == 0)
        {
            animator.SetBool("IsDigging", true);
            animator.SetBool("IsWatering", false); // Đảm bảo tắt tưới nước
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("IsDigging", false);
        }

        // Kiểm tra nhấn T để tưới nước (chỉ khi đứng yên)
        if (Input.GetKeyDown(KeyCode.T) && speed == 0)
        {
            animator.SetBool("IsWatering", true);
            animator.SetBool("IsDigging", false); // Đảm bảo tắt đào
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            animator.SetBool("IsWatering", false);
        }
    }

    void FixedUpdate()
    {
        // Di chuyển player (vẫn cho phép đi chéo)
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
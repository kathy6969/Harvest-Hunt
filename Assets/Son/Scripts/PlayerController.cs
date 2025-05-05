using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection; // Lưu hướng cuối cùng

    [SerializeField] private float moveSpeed = 5f;

    // Particle System cho tia nước bắn lên
    [SerializeField] private ParticleSystem waterSplash;

    // Offset vị trí cho tia nước bắn lên theo từng hướng (dùng Vector2)
    [SerializeField] private Vector2 splashOffsetUp = new Vector2(0, 1);    // Hướng lên
    [SerializeField] private Vector2 splashOffsetDown = new Vector2(0, -1); // Hướng xuống
    [SerializeField] private Vector2 splashOffsetLeft = new Vector2(-1, 0); // Hướng trái
    [SerializeField] private Vector2 splashOffsetRight = new Vector2(1, 0); // Hướng phải

    // Thời gian của animation tưới nước (cần điều chỉnh theo animation của bạn)
    [SerializeField] private float wateringAnimationDuration = 1f; // Ví dụ: 1 giây

    private bool isWateringEffectActive = false; // Trạng thái của effect tưới nước
    private bool isWatering = false; // Trạng thái khóa di chuyển trong lúc tưới nước

    void Start()
    {
        waterSplash.Stop();
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

        // Khởi tạo hướng mặc định (hướng xuống)
        lastDirection = new Vector2(0, -1);
    }

    void Update()
    {
        
        if (FindObjectOfType<InventoryController>().IsInventoryOpen())
            return;
        // Lấy input di chuyển (chỉ khi không đang tưới nước)
        if (!isWatering)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            // Khóa di chuyển trong lúc tưới nước
            movement = Vector2.zero;
        }
        
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

        // Cập nhật hướng cuối cùng
        if (speed > 0)
        {
            lastDirection = new Vector2(directionX, directionY).normalized;
            animator.SetFloat("DirectionX", directionX);
            animator.SetFloat("DirectionY", directionY);

            // Khi nhân vật di chuyển (thức dậy), tắt effect và thoát trạng thái tưới nước
            if (isWateringEffectActive)
            {
                StopWaterSplash();
                animator.SetBool("IsWatering", false);
                isWateringEffectActive = false;
            }
        }

        // Kiểm tra nhấn Space để đào (chỉ khi đứng yên và không đang tưới nước)
        if (Input.GetKeyDown(KeyCode.Space) && speed == 0 && !isWatering)
        {
            animator.SetBool("IsDigging", true);
            animator.SetBool("IsWatering", false);
            // Logic đào đất sẽ thêm sau
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("IsDigging", false);
        }

        // Kiểm tra nhấn T để tưới nước (chỉ khi đứng yên và không đang tưới nước)
        if (Input.GetKeyDown(KeyCode.T) && speed == 0 && !isWatering)
        {
            isWatering = true; // Khóa di chuyển
            animator.SetBool("IsWatering", true);
            animator.SetBool("IsDigging", false);
            StartCoroutine(PlayWaterSplashAfterAnimation());
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            // Không đặt IsWatering thành false ngay lập tức, để animation chạy hết
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Chỉ di chuyển nếu không đang tưới nước
            if (!isWatering)
            {
                rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    // Coroutine để chạy effect sau khi animation tưới nước hoàn tất
    private IEnumerator PlayWaterSplashAfterAnimation()
    {
        // Chờ animation tưới nước hoàn tất
        yield return new WaitForSeconds(wateringAnimationDuration);

        // Kích hoạt effect tia nước bắn lên
        PlayWaterSplash();

        // Đánh dấu effect đang hoạt động
        isWateringEffectActive = true;

        // Lấy thời gian của effect WaterSplash (dựa trên Start Lifetime)
        float effectDuration = waterSplash.main.startLifetime.constant;

        // Chờ effect hoàn tất
        yield return new WaitForSeconds(effectDuration);

        // Dừng effect và thoát trạng thái tưới nước
        StopWaterSplash();
        animator.SetBool("IsWatering", false);
        isWateringEffectActive = false;
        isWatering = false; // Mở khóa di chuyển
    }

    // Hàm kích hoạt tia nước bắn lên và điều chỉnh vị trí
    void PlayWaterSplash()
    {
        if (waterSplash == null) return;

        // Dừng Particle System trước khi điều chỉnh
        waterSplash.Stop();

        // Điều chỉnh vị trí của WaterSplash dựa trên hướng
        Vector2 splashOffset = Vector2.zero;
        if (lastDirection.y > 0) // Up
        {
            splashOffset = splashOffsetUp;
        }
        else if (lastDirection.y < 0) // Down
        {
            splashOffset = splashOffsetDown;
        }
        else if (lastDirection.x < 0) // Left
        {
            splashOffset = splashOffsetLeft;
        }
        else if (lastDirection.x > 0) // Right
        {
            splashOffset = splashOffsetRight;
        }

        // Đặt vị trí tương đối của WaterSplash (vì nó là con của Player)
        waterSplash.transform.localPosition = splashOffset;

        // Phát Particle System
        waterSplash.Play();
    }

    // Hàm dừng tia nước bắn lên
    void StopWaterSplash()
    {
        if (waterSplash != null)
        {
            waterSplash.Stop();
            isWateringEffectActive = false;
        }
    }
}
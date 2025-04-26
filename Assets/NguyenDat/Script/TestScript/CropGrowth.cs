using UnityEngine;

public class CropGrowth : MonoBehaviour
{
    public Sprite[] growthStages; // 4 sprites cho các giai đoạn
    public float timeBetweenStages = 5f; // thời gian giữa các giai đoạn (test)
    public GameObject harvestItemPrefab; // prefab item khi thu hoạch

    private int currentStage = 0;
    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private bool isFullyGrown = false;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        if (growthStages.Length > 0)
            spriteRenderer.sprite = growthStages[0];
    }

    void Update()
    {
        // Tự động chuyển giai đoạn theo thời gian
        if (!isFullyGrown)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenStages)
            {
                timer = 0f;
                currentStage++;

                if (currentStage < growthStages.Length)
                {
                    spriteRenderer.sprite = growthStages[currentStage];
                }
                else
                {
                    isFullyGrown = true;
                }
            }
        }

        // Khi đã trưởng thành, cho phép thu hoạch bằng chuột phải
        if (isFullyGrown && Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                Harvest();
            }
        }
    }

    void Harvest()
    {
        if (harvestItemPrefab != null)
        {
            Instantiate(harvestItemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Xóa cây sau khi thu hoạch
    }
}

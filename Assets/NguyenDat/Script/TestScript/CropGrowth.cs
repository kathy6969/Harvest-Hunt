using UnityEngine;
using System.Collections;

public class CropGrowth : MonoBehaviour
{
    public Sprite[] growthStages;           // Các giai đoạn phát triển (4 sprites)
    public float timeBetweenStages = 5f;    // Thời gian giữa các giai đoạn
    public GameObject harvestItemPrefab;    // Prefab khi thu hoạch

    private int currentStage = 0;
    private SpriteRenderer spriteRenderer;
    private bool isFullyGrown = false;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        if (growthStages.Length > 0)
        {
            spriteRenderer.sprite = growthStages[0];
            StartCoroutine(Grow());
        }
    }

    IEnumerator Grow()
    {
        while (currentStage < growthStages.Length - 1)
        {
            yield return new WaitForSeconds(timeBetweenStages);

            currentStage++;
            spriteRenderer.sprite = growthStages[currentStage];
        }

        // Khi đã đạt giai đoạn cuối
        isFullyGrown = true;
    }

    void Update()
    {
        // Chỉ cho thu hoạch nếu đã trưởng thành
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

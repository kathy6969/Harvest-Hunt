using UnityEngine;

public class Plant : MonoBehaviour
{
    public enum GrowthStage { Stage1, Stage2, Stage3, Stage4 }

    public GrowthStage currentStage = GrowthStage.Stage1;
    public Sprite[] stageSprites;
    public float[] growthTimes;
    public GameObject harvestItemPrefab;
    public Transform itemParent;

    private GameObject currentHarvestItem;
    private bool hasHarvested = false;
    private bool isReadyToHarvest = false;

    private float currentGameTime = 0f;
    private TimeManager timeManager;
    private SpriteRenderer spriteRenderer;

    private int lastAdvancedStage = -1; // lưu stage cuối cùng đã advance

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    void Update()
    {
        if (timeManager == null) return;

        currentGameTime = timeManager.currentTime;

        // Giai đoạn phát triển
        for (int i = 0; i < growthTimes.Length; i++)
        {
            if ((int)currentStage == i && currentGameTime >= growthTimes[i] && lastAdvancedStage != i)
            {
                AdvanceStage();
                lastAdvancedStage = i; // ghi nhận đã advance stage này
                break;
            }
        }


        // Nếu sẵn sàng thu hoạch và chưa sinh item
        if (isReadyToHarvest && !hasHarvested && currentHarvestItem == null)
        {
            Harvest();
        }

        // Reset cờ sau khi item đã bị nhặt hoặc destroy
        if (hasHarvested && currentHarvestItem == null)
        {
            hasHarvested = false;
            isReadyToHarvest = false;
            // 🌱 Reset lại để cây có thể phát triển lại từ Stage3
            lastAdvancedStage = (int)currentStage - 1;
        }
    }

    void AdvanceStage()
    {
        if (hasHarvested && currentHarvestItem != null)
        {
            // Chặn phát triển nếu còn item chưa nhặt
            return;
        }

        if (currentStage < GrowthStage.Stage4)
        {
            currentStage++;
            UpdateVisual();

            if (currentStage == GrowthStage.Stage4)
            {
                isReadyToHarvest = true;
                Harvest(); // ✅ Chỉ gọi ở đây
            }
        }
    }



    void Harvest()
    {
        Debug.Log("Đã gọi Harvest(), stage hiện tại: " + currentStage);

        if (harvestItemPrefab == null || currentHarvestItem != null) return;

        // Sinh item
        currentHarvestItem = Instantiate(harvestItemPrefab, transform.position, Quaternion.identity);
        currentHarvestItem.transform.SetParent(itemParent, true);

        // Lùi stage
        currentStage = GrowthStage.Stage3;
        Debug.Log("Sau khi gán Stage3, currentStage = " + currentStage);

        UpdateVisual();

        // Đánh dấu đã thu hoạch
        hasHarvested = true;
        Destroy(currentHarvestItem, 10f);
    }

    void UpdateVisual()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if ((int)currentStage < stageSprites.Length)
        {
            spriteRenderer.sprite = stageSprites[(int)currentStage];
            Debug.Log("Sprite cập nhật: " + stageSprites[(int)currentStage].name);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy sprite phù hợp với stage " + currentStage);
        }
    }
}

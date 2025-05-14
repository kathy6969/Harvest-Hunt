using System.Reflection;
using UnityEngine;

public class CropRecovery : MonoBehaviour
{
    public float dropOffsetY = 0.8f;

    private bool isRecovering = false;
    private CropGrowth cropGrowth;

    void Start()
    {
        cropGrowth = GetComponent<CropGrowth>();
    }

    void OnDisable()
    {
        if (isRecovering || cropGrowth == null || !gameObject.scene.IsValid())
            return;

        isRecovering = true;

        // Clone lại cây
        GameObject newCrop = Instantiate(gameObject, transform.position, Quaternion.identity);

        // Reset trạng thái để cây tiếp tục phát triển
        var newCropGrowth = newCrop.GetComponent<CropGrowth>();
        var type = typeof(CropGrowth);

        int previousStage = Mathf.Clamp(
            cropGrowth.growthStages.Length - 2, 0, cropGrowth.growthStages.Length - 1
        );

        type.GetField("currentStage", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(newCropGrowth, previousStage);
        type.GetField("isFullyGrown", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(newCropGrowth, false);
        type.GetField("timer", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(newCropGrowth, 0f);

        var renderer = newCrop.GetComponent<SpriteRenderer>();
        renderer.sprite = newCropGrowth.growthStages[previousStage];

        // Di chuyển vật phẩm xuống
        GameObject item = DetectHarvestItemNear(transform.position);
        if (item != null)
            item.transform.position += Vector3.down * dropOffsetY;

        Debug.Log("Cây đã được khôi phục và phát triển lại từ stage " + previousStage);
    }


    GameObject DetectHarvestItemNear(Vector3 pos)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.8f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("HarvestItem") && hit.gameObject != this.gameObject)
                return hit.gameObject;
        }
        return null;
    }
}

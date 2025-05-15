using UnityEngine;

public class CropGrowth : MonoBehaviour
{
    public Sprite[] growthStages;
    public float timeBetweenStages = 5f;
    public GameObject harvestItemPrefab;

    private int currentStage = 0;
    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private bool isFullyGrown = false;
    private Camera mainCamera;
    private Vector3Int cellPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        if (growthStages.Length > 0)
            spriteRenderer.sprite = growthStages[0];
    }

    void Update()
    {
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

        FarmManager.Instance.RemoveCropAt(cellPosition);
        Destroy(gameObject);
    }

    public void SetCellPosition(Vector3Int pos)
    {
        cellPosition = pos;
    }
}

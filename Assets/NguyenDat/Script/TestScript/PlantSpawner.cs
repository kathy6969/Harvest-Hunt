using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantSpawner : MonoBehaviour
{
    public GameObject plantPrefab;
    public Grid grid; // Gán qua Inspector
    public Camera mainCamera;

    private SeedFollowMouse followScript;

    void Start()
    {
        followScript = GetComponent<SeedFollowMouse>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        // Tự động tìm Grid ở cha hoặc trong scene
        if (grid == null)
            grid = GetComponentInParent<Grid>();
        if (grid == null)
            grid = FindObjectOfType<Grid>();

        if (grid == null)
            Debug.LogError("Không tìm thấy Grid trong PlantSpawner!");
    }

    void Update()
    {
        if (followScript != null && followScript.IsFollowing())
        {
            if (Input.GetMouseButtonDown(1)) // Chuột phải
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;

                Vector3Int cellPos = grid.WorldToCell(mouseWorldPos);
                Vector3 spawnPos = grid.GetCellCenterWorld(cellPos);

                // Kiểm tra: có thể trồng và chưa có cây
                if (FarmManager.Instance.IsTileHoed(cellPos) && !FarmManager.Instance.HasCrop(cellPos))
                {
                    GameObject newPlant = Instantiate(plantPrefab, spawnPos, Quaternion.identity);

                    // (Không cần gọi SetCellPosition nếu CropGrowth không có)
                    FarmManager.Instance.AddCrop(cellPos, newPlant);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject plantPrefab;      // Prefab cây (kéo vào từ Inspector)
    private Camera mainCamera;
    private Tilemap tilemap;
    private SeedFollowMouse followScript;
    private PauseMenu plantManager;


    void Start()
    {
        mainCamera = Camera.main;
        tilemap = FindObjectOfType<Tilemap>();
        followScript = GetComponent<SeedFollowMouse>();
        plantManager = FindObjectOfType<PauseMenu>();

    }

    void Update()
    {
        // Chỉ cho phép trồng cây khi đang follow chuột
        if (followScript != null && followScript.IsFollowing())
        {
            if (Input.GetMouseButtonDown(1)) // Chuột phải
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;

                Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
                Vector3 spawnPos = tilemap.GetCellCenterWorld(cellPos);

                //Instantiate(plantPrefab, spawnPos, Quaternion.identity);
                GameObject newPlant = Instantiate(plantPrefab, spawnPos, Quaternion.identity);

                // 🔥 Thêm cây vào danh sách cây đã trồng
                if (plantManager != null)
                {
                    plantManager.allPlantedObjects.Add(newPlant);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy PlantManager!");
                }
            }
        }
    }
}

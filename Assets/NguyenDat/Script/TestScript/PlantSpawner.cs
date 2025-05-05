using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject plantPrefab;      // Prefab cây (kéo vào từ Inspector)
    private Camera mainCamera;
    private Tilemap hoedTilemap;
    private SeedFollowMouse followScript;

    void Start()
    {
        mainCamera = Camera.main;

        // Tìm tilemap có tag "Hoed"
        GameObject hoedTilemapObj = GameObject.FindGameObjectWithTag("Hoed");
        if (hoedTilemapObj != null)
        {
            hoedTilemap = hoedTilemapObj.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Không tìm thấy Tilemap có tag 'Hoed'!");
        }

        followScript = GetComponent<SeedFollowMouse>();
    }

    void Update()
    {
        if (followScript != null && followScript.IsFollowing())
        {
            if (Input.GetMouseButtonDown(1) && hoedTilemap != null)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;

                Vector3Int cellPos = hoedTilemap.WorldToCell(mouseWorldPos);

                // Kiểm tra có tile tại ô đó hay không
                if (hoedTilemap.HasTile(cellPos))
                {
                    Vector3 spawnPos = hoedTilemap.GetCellCenterWorld(cellPos);
                    Instantiate(plantPrefab, spawnPos, Quaternion.identity);
                }
                else
                {
                    Debug.Log("Không có đất đã cuốc ở đây, không thể trồng.");
                }
            }
        }
    }
}

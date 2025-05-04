using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeedFollowMouse : MonoBehaviour
{
    private bool isFollowing = false;
    private Camera mainCamera;
    private Tilemap tilemap;

    void Start()
    {
        mainCamera = Camera.main;

        // Tự động tìm Tilemap trong scene (lấy tilemap đầu tiên tìm thấy)
        tilemap = FindObjectOfType<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogError("Không tìm thấy Tilemap nào trong scene!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click chuột trái
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                isFollowing = !isFollowing;
            }
        }

        if (isFollowing && tilemap != null)
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
            Vector3 snappedPos = tilemap.GetCellCenterWorld(cellPos);

            transform.position = snappedPos;
        }
    }

    public bool IsFollowing()
    {
        return isFollowing;
    }
}

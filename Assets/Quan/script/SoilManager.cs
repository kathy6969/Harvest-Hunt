using UnityEngine;
using UnityEngine.Tilemaps;

public class SoilManager : MonoBehaviour
{
    public Tilemap soilTilemap; // Kéo Tilemap vào Inspector
    public TileBase normalSoilTile; // Tile đất thường
    public TileBase dugSoilTile;    // Tile đất đã cuốc (màu nâu)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click chuột trái
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DigAt(mousePos);
        }
    }

    void DigAt(Vector2 worldPos)
    {
        Vector3Int cellPos = soilTilemap.WorldToCell(worldPos);

        if (soilTilemap.GetTile(cellPos) == normalSoilTile)
        {
            soilTilemap.SetTile(cellPos, dugSoilTile);
            Debug.Log($"Đã cuốc đất tại: {cellPos}");
        }
    }
}
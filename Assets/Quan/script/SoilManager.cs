// Script: DiggingHandler.cs
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
public class DiggingHandler : MonoBehaviour
{
    [Header("Cài đặt Tile")]
    public TileBase normalDirtTile;  // Tile đất thường
    public TileBase dugDirtTile;    // Tile đất đã đào

    [Header("Tham chiếu Tilemap")]
    public Tilemap dugDirtTilemap;  // Tilemap chứa đất đã đào

    private Tilemap currentTilemap;
    private TilemapCollider2D tilemapCollider;

    private void Awake()
    {
        currentTilemap = GetComponent<Tilemap>();
        tilemapCollider = GetComponent<TilemapCollider2D>();

        // Thiết lập Collider là Trigger
        tilemapCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dig"))
        {
            Vector3 hitPosition = other.transform.position;
            Vector3Int cellPosition = currentTilemap.WorldToCell(hitPosition);

            // Chỉ xử lý nếu có tile tại vị trí này
            if (currentTilemap.HasTile(cellPosition))
            {
                DigTile(cellPosition);
            }
        }
    }

    private void DigTile(Vector3Int cellPosition)
    {
        // Xóa tile đất thường
        currentTilemap.SetTile(cellPosition, null);

        // Thêm tile đất đã đào vào Tilemap khác
        dugDirtTilemap.SetTile(cellPosition, dugDirtTile);

        Debug.Log($"Đã đào tại vị trí ô: {cellPosition}");
    }
}
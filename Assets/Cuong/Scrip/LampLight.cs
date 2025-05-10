using UnityEngine;
using UnityEngine.Tilemaps;

public class LampLight : MonoBehaviour
{
    public Tilemap lightTilemap;       // Gán từ Editor
    public TileBase lightTile;         // Tile ánh sáng (sprite mờ)

    void Start()
    {
        Vector3 worldPos = transform.position;
        Vector3Int centerCell = lightTilemap.WorldToCell(worldPos);

        LightAround(centerCell);
    }

    void LightAround(Vector3Int center)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                Vector3Int tilePos = new Vector3Int(center.x + dx, center.y + dy, 0);
                lightTilemap.SetTile(tilePos, lightTile);
            }
        }
    }
}

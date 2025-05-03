using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeSystem : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase grassTile;    // Tile cỏ
    public TileBase hoedTile;     // Tile đã cuốc
    public Transform player;
    public LineRenderer lineRenderer;

    private Vector3Int lastCell;

    void Start()
    {
        lineRenderer.positionCount = 5;
        lineRenderer.loop = false;
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.useWorldSpace = true;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
        Vector3Int playerCell = tilemap.WorldToCell(player.position);

        bool isInRange = Mathf.Abs(cellPos.x - playerCell.x) <= 1 && Mathf.Abs(cellPos.y - playerCell.y) <= 1;

        if (cellPos != lastCell)
        {
            lastCell = cellPos;
            DrawHighlight(cellPos, isInRange ? Color.green : Color.red);
        }

        if (Input.GetMouseButtonDown(0))  // Chuột trái
        {
            if (isInRange)
            {
                TileBase currentTile = tilemap.GetTile(cellPos);
                if (currentTile == grassTile)
                {
                    tilemap.SetTile(cellPos, hoedTile);  // Đào cỏ thành đất
                }

            }
            else
            {
                Debug.Log("Ô ngoài phạm vi cuốc!");
            }
        }
    }

    void DrawHighlight(Vector3Int cell, Color color)
    {
        Vector3 cellWorldPos = tilemap.GetCellCenterWorld(cell);
        Vector3 cellSize = tilemap.cellSize;

        Vector3 bottomLeft = cellWorldPos + new Vector3(-cellSize.x / 2, -cellSize.y / 2, 0);
        Vector3 bottomRight = cellWorldPos + new Vector3(cellSize.x / 2, -cellSize.y / 2, 0);
        Vector3 topRight = cellWorldPos + new Vector3(cellSize.x / 2, cellSize.y / 2, 0);
        Vector3 topLeft = cellWorldPos + new Vector3(-cellSize.x / 2, cellSize.y / 2, 0);

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.SetPositions(new Vector3[] { bottomLeft, bottomRight, topRight, topLeft, bottomLeft });
    }
}

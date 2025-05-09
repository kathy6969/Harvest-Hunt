using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeSystem : MonoBehaviour
{
    public Tilemap tilemap1;
    public Tilemap tilemap2;
    public TileBase grassTile;
    public TileBase hoedTile;
    public Transform player;
    public LineRenderer lineRenderer;

    private Vector3Int lastCell;
    private PlayerController playerController;

    void Start()
    {
        lineRenderer.positionCount = 5;
        lineRenderer.loop = false;
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.useWorldSpace = true;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogError("Không tìm thấy PlayerController trên Player!");
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap1.WorldToCell(mouseWorldPos);
        Vector3Int playerCell = tilemap1.WorldToCell(player.position);

        int dx = Mathf.Abs(cellPos.x - playerCell.x);
        int dy = Mathf.Abs(cellPos.y - playerCell.y);

        // Cho phép đào ô kề bên, bao gồm chéo
        bool isInRange = (dx <= 1 && dy <= 1) && (dx + dy != 0);

        if (cellPos != lastCell)
        {
            lastCell = cellPos;
            DrawHighlight(cellPos, isInRange ? Color.green : Color.red);
        }

        if (Input.GetMouseButtonDown(0))  // Chuột trái
        {
            if (cellPos == playerCell)
            {
                Debug.Log("Không thể cuốc ngay dưới chân!");
                return;
            }

            if (isInRange)
            {
                TileBase currentTile = tilemap1.GetTile(cellPos);
                if (currentTile == grassTile)
                {
                    tilemap1.SetTile(cellPos, null);
                    tilemap2.SetTile(cellPos, hoedTile);
                    FarmManager.Instance.AddHoedTile(cellPos);
                    Debug.Log($"Đã cuốc ô {cellPos}");

                    Vector2Int playerCell2D = new Vector2Int(playerCell.x, playerCell.y);
                    Vector2Int targetCell2D = new Vector2Int(cellPos.x, cellPos.y);
                    playerController.SetDigDirection(playerCell2D, targetCell2D);
                }
                else
                {
                    Debug.Log("Ô này không phải cỏ!");
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
        Vector3 cellWorldPos = tilemap1.GetCellCenterWorld(cell);
        Vector3 cellSize = tilemap1.cellSize;

        Vector3 bottomLeft = cellWorldPos + new Vector3(-cellSize.x / 2, -cellSize.y / 2, 0);
        Vector3 bottomRight = cellWorldPos + new Vector3(cellSize.x / 2, -cellSize.y / 2, 0);
        Vector3 topRight = cellWorldPos + new Vector3(cellSize.x / 2, cellSize.y / 2, 0);
        Vector3 topLeft = cellWorldPos + new Vector3(-cellSize.x / 2, cellSize.y / 2, 0);

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.SetPositions(new Vector3[] { bottomLeft, bottomRight, topRight, topLeft, bottomLeft });
    }
}

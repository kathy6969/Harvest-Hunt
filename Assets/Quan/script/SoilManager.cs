using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
public class DiggingSystem : MonoBehaviour
{
    [Header("Tile Settings")]
    public TileBase dugDirtTile;
    public Tilemap dugDirtTilemap;

    [Header("Digging Settings")]
    public float digCooldown = 0.1f;
    public Vector2 digSize = new Vector2(0.8f, 0.8f);

    private Tilemap _normalDirtTilemap;
    private TilemapCollider2D _collider;
    private float _lastDigTime;

    private void Awake()
    {
        _normalDirtTilemap = GetComponent<Tilemap>();
        _collider = GetComponent<TilemapCollider2D>();
        _collider.isTrigger = true;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Khi giữ chuột trái
        {
            TryDigAtMousePosition();
        }
    }

    private void TryDigAtMousePosition()
    {
        if (Time.time - _lastDigTime < digCooldown) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = _normalDirtTilemap.WorldToCell(mouseWorldPos);

        // Kiểm tra trong phạm vi digSize
        Collider2D hit = Physics2D.OverlapBox(mouseWorldPos, digSize, 0);
        if (hit != null && hit.CompareTag("Dig"))
        {
            if (_normalDirtTilemap.HasTile(cellPos))
            {
                DigTile(cellPos);
                _lastDigTime = Time.time;
            }
        }
    }

    private void DigTile(Vector3Int cellPosition)
    {
        _normalDirtTilemap.SetTile(cellPosition, null);
        dugDirtTilemap.SetTile(cellPosition, dugDirtTile);
    }

    // Vẽ khung debug
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(mousePos, digSize);
    }
}
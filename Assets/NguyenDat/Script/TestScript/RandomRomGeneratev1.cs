using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomRomGeneratev1 : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tile groundTile;
    public Tile wallTile;

    public int minRoomCount = 5;
    public int maxRoomCount = 10;
    public Vector2Int roomSizeMinMax = new Vector2Int(5, 10);
    public int mapRange = 30;

    private Dictionary<Vector2Int, int> map = new();
    private List<RectInt> rooms = new();

    public GameObject monsterPrefab;
    public int MinMonsterCount;
    public int MaxMonsterCount;

    void Start()
    {
        GenerateRooms();
        DrawMap();
    }

    void GenerateRooms()
    {
        map.Clear();
        rooms.Clear();

        // Tạo phòng gốc tại (0,0)
        RectInt startRoom = new RectInt(-5, -5, 10, 10);
        rooms.Add(startRoom);
        CarveRoom(startRoom);

        int roomCount = Random.Range(minRoomCount, maxRoomCount + 1);

        for (int i = 1; i < roomCount; i++)
        {
            RectInt newRoom;
            int tries = 0;

            do
            {
                tries++;
                int width = Random.Range(roomSizeMinMax.x, roomSizeMinMax.y + 1);
                int height = Random.Range(roomSizeMinMax.x, roomSizeMinMax.y + 1);
                Vector2Int pos = new Vector2Int(
                    Random.Range(-mapRange, mapRange),
                    Random.Range(-mapRange, mapRange)
                );
                newRoom = new RectInt(pos, new Vector2Int(width, height));
            }
            while (RoomOverlaps(newRoom) && tries < 10);

            if (tries >= 10) continue;

            rooms.Add(newRoom);
            CarveRoom(newRoom);
        }

        // Nối các phòng: mỗi phòng nối 1–3 phòng gần nhất, không bị cô lập
        HashSet<int> connectedRooms = new();

        for (int i = 1; i < rooms.Count; i++)
        {
            Vector2Int centerA = Vector2Int.RoundToInt(rooms[i].center);
            List<(float dist, int index, Vector2Int center)> nearby = new();

            for (int j = 0; j < rooms.Count; j++)
            {
                if (i == j) continue;
                Vector2Int centerB = Vector2Int.RoundToInt(rooms[j].center);
                float distance = Vector2Int.Distance(centerA, centerB);
                nearby.Add((distance, j, centerB));
            }

            nearby.Sort((a, b) => a.dist.CompareTo(b.dist));

            int connectionCount = Random.Range(1, 4);
            int connectionsMade = 0;

            foreach (var (dist, index, centerB) in nearby)
            {
                if (connectionsMade >= connectionCount) break;

                if (!connectedRooms.Contains(i) || !connectedRooms.Contains(index))
                {
                    ConnectRooms(centerA, centerB);
                    connectedRooms.Add(i);
                    connectedRooms.Add(index);
                    connectionsMade++;
                }
            }

            // Nếu vẫn chưa nối ai, bắt buộc nối ít nhất 1
            if (!connectedRooms.Contains(i) && nearby.Count > 0)
            {
                var fallback = nearby[0];
                ConnectRooms(centerA, fallback.center);
                connectedRooms.Add(i);
                connectedRooms.Add(fallback.index);
            }
        }

        FillWalls();
        SpawnMonstersInRooms();
    }


    void CarveRoom(RectInt room)
    {
        foreach (Vector2Int pos in room.allPositionsWithin)
        {
            map[pos] = 0;
        }
    }

    bool RoomOverlaps(RectInt newRoom)
    {
        foreach (var room in rooms)
        {
            if (room.Overlaps(newRoom))
                return true;
        }
        return false;
    }

    void ConnectRooms(Vector2Int a, Vector2Int b)
    {
        Vector2Int current = a;

        while (current.x != b.x)
        {
            current.x += (b.x > current.x) ? 1 : -1;
            CarveCorridorTile(current, Vector2Int.up); // mở rộng theo trục y
        }

        while (current.y != b.y)
        {
            current.y += (b.y > current.y) ? 1 : -1;
            CarveCorridorTile(current, Vector2Int.right); // mở rộng theo trục x
        }
    }

    // Hành lang 2 tile: đục 1 tile chính + 1 tile sang một phía
    void CarveCorridorTile(Vector2Int pos, Vector2Int offset)
    {
        map[pos] = 0;
        map[pos + offset] = 0;
    }

    void FillWalls()
    {
        HashSet<Vector2Int> toCheck = new(map.Keys);

        foreach (var pos in map.Keys)
        {
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    toCheck.Add(pos + new Vector2Int(x, y));
        }

        foreach (var pos in toCheck)
        {
            if (!map.ContainsKey(pos))
            {
                map[pos] = 1;
            }
        }
    }

    void DrawMap()
    {
        groundTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        foreach (var pair in map)
        {
            if (pair.Value == 0)
                groundTilemap.SetTile((Vector3Int)pair.Key, groundTile);
            else if (pair.Value == 1)
                wallTilemap.SetTile((Vector3Int)pair.Key, wallTile);
        }
    }

    void SpawnMonstersInRooms()
    {
        for (int i = 1; i < rooms.Count; i++) // bỏ qua phòng 0 (phòng xuất phát)
        {
            RectInt room = rooms[i];
            int monsterCount = Random.Range(MinMonsterCount, MaxMonsterCount);

            for (int j = 0; j < monsterCount; j++)
            {
                Vector2Int spawnPos = new Vector2Int(
                    Random.Range(room.xMin + 1, room.xMax - 1),
                    Random.Range(room.yMin + 1, room.yMax - 1)
                );

                Vector3 worldPos = groundTilemap.CellToWorld((Vector3Int)spawnPos) + new Vector3(0.5f, 0.5f, 0);
                Instantiate(monsterPrefab, worldPos, Quaternion.identity);
            }
        }
    }
}

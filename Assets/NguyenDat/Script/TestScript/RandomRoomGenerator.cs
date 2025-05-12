using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomRoomGenerator : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tile groundTile;
    public Tile wallTile;

    public int minRoomCount = 5;
    public int maxRoomCount = 10;
    public Vector2Int roomSizeMinMax = new Vector2Int(5, 10);
    public int mapRange = 30;
    public int irregularSteps = 50;

    private Dictionary<Vector2Int, int> map = new();
    private List<IrregularRoom> rooms = new();

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
        IrregularRoom startRoom = GenerateIrregularRoom(Vector2Int.zero, irregularSteps);
        rooms.Add(startRoom);
        CarveRoom(startRoom);

        int roomCount = Random.Range(minRoomCount, maxRoomCount + 1);

        for (int i = 1; i < roomCount; i++)
        {
            IrregularRoom newRoom;
            int tries = 0;

            do
            {
                tries++;
                Vector2Int pos = new Vector2Int(
                    Random.Range(-mapRange, mapRange),
                    Random.Range(-mapRange, mapRange)
                );
                newRoom = GenerateIrregularRoom(pos, irregularSteps);
            }
            while (RoomOverlaps(newRoom) && tries < 10);

            if (tries >= 10) continue;

            rooms.Add(newRoom);
            CarveRoom(newRoom);
            ConnectRooms(rooms[i - 1].GetCenter(), newRoom.GetCenter());
        }

        FillWalls();
    }

    IrregularRoom GenerateIrregularRoom(Vector2Int startPos, int steps)
    {
        IrregularRoom room = new IrregularRoom();
        Vector2Int currentPos = startPos;
        room.tiles.Add(currentPos);

        Vector2Int[] directions = new Vector2Int[] {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        for (int i = 0; i < steps; i++)
        {
            Vector2Int dir = directions[Random.Range(0, directions.Length)];
            currentPos += dir;

            if (!room.tiles.Contains(currentPos) && IsInMapBounds(currentPos))
            {
                room.tiles.Add(currentPos);
            }
        }

        return room;
    }

    void CarveRoom(IrregularRoom room)
    {
        foreach (Vector2Int pos in room.tiles)
        {
            map[pos] = 0;
        }
    }

    bool RoomOverlaps(IrregularRoom newRoom)
    {
        foreach (var room in rooms)
        {
            foreach (var pos in newRoom.tiles)
            {
                if (room.tiles.Contains(pos))
                    return true;
            }
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

    bool IsInMapBounds(Vector2Int pos)
    {
        return Mathf.Abs(pos.x) <= mapRange && Mathf.Abs(pos.y) <= mapRange;
    }
}

public class IrregularRoom
{
    public List<Vector2Int> tiles = new List<Vector2Int>();

    public Vector2Int GetCenter()
    {
        if (tiles.Count == 0) return Vector2Int.zero;
        int x = 0, y = 0;
        foreach (var t in tiles)
        {
            x += t.x;
            y += t.y;
        }
        return new Vector2Int(x / tiles.Count, y / tiles.Count);
    }
}

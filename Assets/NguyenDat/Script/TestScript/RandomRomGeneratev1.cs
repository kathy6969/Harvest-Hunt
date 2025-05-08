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
            ConnectRooms(Vector2Int.RoundToInt(rooms[i - 1].center), Vector2Int.RoundToInt(newRoom.center));
        }

        FillWalls();
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
}

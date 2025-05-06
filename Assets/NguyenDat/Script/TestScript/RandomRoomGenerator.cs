using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomRoomGenerator : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public TileBase groundTile;
    public TileBase wallTile;

    public int walkLength = 1000;
    public int smoothIterations = 3;

    Dictionary<Vector2Int, int> map = new();

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map.Clear();
        RandomWalkMap();

        for (int i = 0; i < smoothIterations; i++)
        {
            SmoothMap();
        }

        DrawTiles();
    }

    void RandomWalkMap()
    {
        Vector2Int pos = Vector2Int.zero;
        map[pos] = 0;

        for (int i = 0; i < walkLength; i++)
        {
            int dir = Random.Range(0, 4);
            switch (dir)
            {
                case 0: pos += Vector2Int.up; break;
                case 1: pos += Vector2Int.down; break;
                case 2: pos += Vector2Int.left; break;
                case 3: pos += Vector2Int.right; break;
            }

            map[pos] = 0; // sàn
        }

        // Đặt những ô chưa đi qua thành tường bao
        HashSet<Vector2Int> allToCheck = new();
        foreach (var tilePos in map.Keys)
        {
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (x != 0 || y != 0)
                        allToCheck.Add(tilePos + new Vector2Int(x, y));
        }

        foreach (var p in allToCheck)
        {
            if (!map.ContainsKey(p))
                map[p] = 1; // tường
        }
    }

    void SmoothMap()
    {
        Dictionary<Vector2Int, int> newMap = new(map);

        foreach (var kvp in map)
        {
            Vector2Int pos = kvp.Key;
            int wallCount = GetSurroundingWallCount(pos);

            if (wallCount > 4)
                newMap[pos] = 1;
            else if (wallCount < 4)
                newMap[pos] = 0;
        }

        map = newMap;
    }

    int GetSurroundingWallCount(Vector2Int pos)
    {
        int wallCount = 0;
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector2Int neighbor = pos + new Vector2Int(x, y);
                if (!map.ContainsKey(neighbor) || map[neighbor] == 1)
                    wallCount++;
            }
        return wallCount;
    }

    void DrawTiles()
    {
        groundTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        foreach (var kvp in map)
        {
            Vector3Int pos = new Vector3Int(kvp.Key.x, kvp.Key.y, 0);
            if (kvp.Value == 0)
                groundTilemap.SetTile(pos, groundTile);
            else
                wallTilemap.SetTile(pos, wallTile);
        }
    }
}

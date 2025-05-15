using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }

    private HashSet<Vector3Int> hoedTiles = new HashSet<Vector3Int>();       // Ô đất có thể trồng
    private Dictionary<Vector3Int, GameObject> plantedCrops = new Dictionary<Vector3Int, GameObject>(); // Cây đã trồng

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Thêm ô đất có thể trồng
    public void AddHoedTile(Vector3Int cellPos)
    {
        if (!hoedTiles.Contains(cellPos))
        {
            hoedTiles.Add(cellPos);
        }
    }

    // Thêm cây đã trồng
    public void AddCrop(Vector3Int pos, GameObject crop)
    {
        if (!plantedCrops.ContainsKey(pos))
            plantedCrops[pos] = crop;
    }

    // Kiểm tra ô có phải đất đã cuốc
    public bool IsTileHoed(Vector3Int cellPos)
    {
        return hoedTiles.Contains(cellPos);
    }

    // Kiểm tra đã trồng cây ở ô này chưa
    public bool HasCrop(Vector3Int pos)
    {
        return plantedCrops.ContainsKey(pos);
    }

    // Gỡ cây ra khỏi danh sách khi thu hoạch
    public void RemoveCropAt(Vector3Int pos)
    {
        if (plantedCrops.ContainsKey(pos))
        {
            plantedCrops.Remove(pos);
        }
    }
}

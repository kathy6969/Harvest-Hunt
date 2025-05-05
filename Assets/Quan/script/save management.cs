using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveLoadSystem : MonoBehaviour
{
    public Tilemap tilemap1; // grass layer
    public Tilemap tilemap2; // hoed layer
    public TileBase grassTile;
    public TileBase hoedTile;

    public Transform player;

    public Button slot1Button, slot2Button, slot3Button, saveButton, loadButton;

    private int selectedSlot = -1;

    void Start()
    {
        slot1Button.onClick.AddListener(() => SelectSlot(1));
        slot2Button.onClick.AddListener(() => SelectSlot(2));
        slot3Button.onClick.AddListener(() => SelectSlot(3));

        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
    }

    void SelectSlot(int slot)
    {
        selectedSlot = slot;
        Debug.Log("Đã chọn slot " + slot);

        slot1Button.GetComponent<Image>().color = Color.white;
        slot2Button.GetComponent<Image>().color = Color.white;
        slot3Button.GetComponent<Image>().color = Color.white;

        if (slot == 1) slot1Button.GetComponent<Image>().color = Color.green;
        if (slot == 2) slot2Button.GetComponent<Image>().color = Color.green;
        if (slot == 3) slot3Button.GetComponent<Image>().color = Color.green;
    }

    void SaveGame()
    {
        if (selectedSlot == -1)
        {
            Debug.LogWarning("Bạn chưa chọn slot để lưu!");
            return;
        }

        SaveTilemap(tilemap1, "Tilemap1_Slot" + selectedSlot, grassTile.name);
        SaveTilemap(tilemap2, "Tilemap2_Slot" + selectedSlot, hoedTile.name);

        // Save player position
        PlayerPrefs.SetFloat("PlayerPosX_Slot" + selectedSlot, player.position.x);
        PlayerPrefs.SetFloat("PlayerPosY_Slot" + selectedSlot, player.position.y);

        Debug.Log("Đã lưu slot " + selectedSlot);
    }

    void LoadGame()
    {
        if (selectedSlot == -1)
        {
            Debug.LogWarning("Bạn chưa chọn slot để load!");
            return;
        }

        LoadTilemap(tilemap1, "Tilemap1_Slot" + selectedSlot, grassTile);
        LoadTilemap(tilemap2, "Tilemap2_Slot" + selectedSlot, hoedTile);

        // Load player position
        float x = PlayerPrefs.GetFloat("PlayerPosX_Slot" + selectedSlot, player.position.x);
        float y = PlayerPrefs.GetFloat("PlayerPosY_Slot" + selectedSlot, player.position.y);
        player.position = new Vector3(x, y, player.position.z);

        Debug.Log("Đã load slot " + selectedSlot);
    }

    void SaveTilemap(Tilemap map, string keyPrefix, string tileName)
    {
        BoundsInt bounds = map.cellBounds;
        List<string> savedPositions = new List<string>();

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = map.GetTile(pos);
            if (tile != null)
            {
                savedPositions.Add($"{pos.x},{pos.y}");
            }
        }

        string data = string.Join("|", savedPositions);
        PlayerPrefs.SetString(keyPrefix, data);
    }

    void LoadTilemap(Tilemap map, string keyPrefix, TileBase tileToSet)
    {
        map.ClearAllTiles();

        string data = PlayerPrefs.GetString(keyPrefix, "");
        if (string.IsNullOrEmpty(data)) return;

        string[] positions = data.Split('|');
        foreach (string posStr in positions)
        {
            string[] parts = posStr.Split(',');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);

            map.SetTile(new Vector3Int(x, y, 0), tileToSet);
        }
    }
}

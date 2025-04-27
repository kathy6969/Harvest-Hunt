using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class PlantPrefabData
{
    public string plantType;
    public GameObject prefab;
}

public class PauseMenu : MonoBehaviour
{
    public GameObject player;   // Kéo Player vô đây
    public List<PlantPrefabData> allPlantPrefabs;
    public List<GameObject> allPlantedObjects = new List<GameObject>();

    public GameObject pauseMenuUI;
    public Button saveButton;
    public Button loadButton;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false); // Ẩn Menu lúc đầu

        // Gán chức năng cho nút
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseMenuUI.SetActive(isPaused);

            if (isPaused)
            {
                Time.timeScale = 0f; // Dừng game
            }
            else
            {
                Time.timeScale = 1f; // Tiếp tục game
            }
        }
    }

    public void SaveGame()
    {
        SaveManager.SaveGame(player.transform.position, allPlantedObjects);
    }

    public void LoadGame()
    {
        PlayerData data = SaveManager.LoadGame();
        if (data == null) return;

        // Load vị trí player
        player.transform.position = data.GetPlayerPosition();

        // Xóa cây cũ
        foreach (var plant in allPlantedObjects)
        {
            Destroy(plant);
        }
        allPlantedObjects.Clear();

        // Load lại cây
        foreach (var plantData in data.plantList)
        {
            GameObject prefab = GetPlantPrefabByType(plantData.plantType);
            if (prefab != null)
            {
                GameObject newPlant = Instantiate(prefab, plantData.GetPosition(), Quaternion.identity);
                Plant plantScript = newPlant.GetComponent<Plant>();
                if (plantScript != null)
                {
                    plantScript.plantType = plantData.plantType;
                }
                allPlantedObjects.Add(newPlant);
            }
            else
            {
                Debug.LogError("Không tìm thấy Prefab cho loại cây: " + plantData.plantType);
            }
        }
    }

    GameObject GetPlantPrefabByType(string type)
    {
        foreach (var item in allPlantPrefabs)
        {
            if (item.plantType == type)
                return item.prefab;
        }
        return null;
    }
}

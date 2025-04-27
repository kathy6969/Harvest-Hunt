using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    static string savePath = Application.persistentDataPath + "/savegame.json";

    public static void SaveGame(Vector3 playerPosition, List<GameObject> allPlants)
    {
        PlayerData data = new PlayerData(playerPosition);

        foreach (var plant in allPlants)
        {
            Plant plantScript = plant.GetComponent<Plant>();
            if (plantScript != null)
            {
                data.plantList.Add(new PlantData(plant.transform.position, plantScript.plantType));
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved at: " + savePath);
    }

    public static PlayerData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return null;
        }

        string json = File.ReadAllText(savePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        return data;
    }
}

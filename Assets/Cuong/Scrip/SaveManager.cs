using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SerializableVector3 playerPosition;
    public List<PlantData> plants = new List<PlantData>();
}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

public class SaveManager : MonoBehaviour
{
    public static string saveFilePath;

    private static void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public static void SaveGame(Vector3 playerPosition, List<GameObject> allPlants)
    {
        SaveData saveData = new SaveData();
        saveData.playerPosition = new SerializableVector3(playerPosition);

        foreach (GameObject plantObj in allPlants)
        {
            if (plantObj == null) continue;

            PlantData plantData = new PlantData();//err
            plantData.position = new SerializableVector3(plantObj.transform.position);
            plantData.plantType = plantObj.name.Replace("(Clone)", "").Trim();

            saveData.plants.Add(plantData);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Đã lưu game vào: " + saveFilePath);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Đã load game từ: " + saveFilePath);
            return saveData;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy file lưu game!");
            return null;
        }
    }
}

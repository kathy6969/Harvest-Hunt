using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string savePath = Application.persistentDataPath + "/playerdata.json";

    public static void SavePlayerPosition(Vector3 position)
    {
        PlayerData data = new PlayerData();
        data.SetPosition(position);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Đã lưu vị trí tại: " + savePath);
    }

    public static Vector3 LoadPlayerPosition()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data.GetPosition();
        }
        else
        {
            Debug.Log("Chưa có dữ liệu vị trí.");
            return Vector3.zero;
        }
    }
}

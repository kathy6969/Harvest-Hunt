using UnityEngine;
using System.IO;

public class SaveViewer : MonoBehaviour
{
    void Start()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        if (File.Exists(path))
        {
            Debug.Log("Save Data: " + File.ReadAllText(path));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button saveButton;
    public Button loadButton;
    public GameObject player; // Gán player vào đây từ Inspector

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

    void SaveGame()
    {
        SaveManager.SavePlayerPosition(player.transform.position);
        Debug.Log("Đã lưu game!");
    }

    void LoadGame()
    {
        Vector3 loadedPos = SaveManager.LoadPlayerPosition();
        player.transform.position = loadedPos;
        Debug.Log("Đã load game!");
    }
}

using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    public Canvas targetCanvas;    // Kéo Canvas vào đây
    public HoeSystem hoeSystem;    // Kéo script HoeSystem vào đây

    private bool isVisible = false;

    void Start()
    {
        if (targetCanvas != null)
        {
            targetCanvas.enabled = isVisible;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isVisible = !isVisible;

        if (targetCanvas != null)
        {
            targetCanvas.enabled = isVisible;
        }

        // Nếu bật menu -> dừng game, tắt input
        if (isVisible)
        {
            Time.timeScale = 0f;               // Pause game (dừng Update, physics)
            hoeSystem.enabled = false;         // Tắt script HoeSystem để không click
        }
        else
        {
            Time.timeScale = 1f;               // Resume game
            hoeSystem.enabled = true;          // Bật lại script HoeSystem
        }
    }
}

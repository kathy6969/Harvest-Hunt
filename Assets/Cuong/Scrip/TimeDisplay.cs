using UnityEngine;
using UnityEngine.UI; // Nếu dùng Text (Legacy)
using TMPro;          // Nếu dùng TextMeshPro

public class TimeDisplay : MonoBehaviour
{
    // Text thường
    //public Text timeText;

    // Nếu dùng TextMeshPro thì dùng dòng sau thay thế dòng trên:
    public TMP_Text timeText;

    void OnEnable()
    {
        TimeManager.OnTimeChanged += UpdateTimeDisplay;
    }

    void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateTimeDisplay;
    }

    void UpdateTimeDisplay(float gameHour)
    {
        int hour = Mathf.FloorToInt(gameHour);
        int minute = Mathf.FloorToInt((gameHour - hour) * 60f);
        timeText.text = $"{hour:00}:{minute:00}";
    }
}

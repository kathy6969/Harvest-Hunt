using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeManager : MonoBehaviour
{
    public float realSecondsPerGameDay = 90f;
    public float currentTime = 0f;

    public int day = 1;
    public int month = 1;

    private int[] daysInMonths = new int[]
    {
        31, 28, 31, 30, 31, 30,
        31, 31, 30, 31, 30, 31
    };

    public TMP_Text dateText;

    public delegate void TimeChanged(float gameHour);
    public static event TimeChanged OnTimeChanged;

    public delegate void DateChanged(string season, int day, int month);
    public static event DateChanged OnDateChanged;

    void Start()
    {
        string season = GetSeason(month);

        OnDateChanged?.Invoke(season, day, month);

        if (dateText != null)
        {
            dateText.text = $"{season} - {day}/{month}";
        }
    }

    void Update()
    {
        currentTime += (24f / realSecondsPerGameDay) * Time.deltaTime;

        if (currentTime >= 24f)
        {
            currentTime -= 24f;
            AdvanceOneDay();
        }

        OnTimeChanged?.Invoke(currentTime);
    }

    void AdvanceOneDay()
    {
        day++;

        if (day > daysInMonths[month - 1])
        {
            day = 1;
            month++;

            if (month > 12)
            {
                month = 1;
            }
        }

        string season = GetSeason(month);

        OnDateChanged?.Invoke(season, day, month);

        if (dateText != null)
        {
            dateText.text = $"{season} - {day}/{month}";
        }
    }

    string GetSeason(int month)
    {
        if (month >= 3 && month <= 5) return "Spring";
        if (month >= 6 && month <= 8) return "Summer";
        if (month >= 9 && month <= 11) return "Fall";
        return "Winter"; // Tháng 12, 1, 2
    }
}

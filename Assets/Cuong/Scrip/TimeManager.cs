using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float realSecondsPerGameDay = 30f; // 24p ngoài đời = 1 ngày game
    public float currentTime; // thời gian hiện tại trong ngày game (0 đến 24)

    public delegate void TimeChanged(float gameHour);
    public static event TimeChanged OnTimeChanged;

    void Update()
    {
        // Tăng thời gian theo thời gian thực
        currentTime += (24f / realSecondsPerGameDay) * Time.deltaTime;

        if (currentTime >= 24f)
            currentTime -= 24f;
        Debug.Log("Giờ trong game: " + currentTime);
        OnTimeChanged?.Invoke(currentTime);
    }
}


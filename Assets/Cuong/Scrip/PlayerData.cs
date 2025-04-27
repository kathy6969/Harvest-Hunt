using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public List<PlantData> plantList = new List<PlantData>();

    public PlayerData(Vector3 playerPos)
    {
        playerPosX = playerPos.x;
        playerPosY = playerPos.y;
        playerPosZ = playerPos.z;
    }

    public Vector3 GetPlayerPosition()
    {
        return new Vector3(playerPosX, playerPosY, playerPosZ);
    }
}

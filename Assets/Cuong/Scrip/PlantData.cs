using UnityEngine;

[System.Serializable]
public class PlantData
{
    public string plantType;
    public float posX;
    public float posY;
    public float posZ;

    public PlantData(Vector3 position, string type)
    {
        posX = position.x;
        posY = position.y;
        posZ = position.z;
        plantType = type;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(posX, posY, posZ);
    }
}

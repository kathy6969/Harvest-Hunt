using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float x;
    public float y;
    public float z;

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }

    public void SetPosition(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }
}

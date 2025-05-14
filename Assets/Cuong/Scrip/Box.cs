using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Transform plantSpawnPoint;
    public Transform tileContainer;  // GameObject chứa cây và item
    public GameObject plantPrefab;

    public void PlantSeed()
    {
        GameObject plant = Instantiate(plantPrefab, plantSpawnPoint.position, Quaternion.identity);

        // Set cả cây và item đều làm con của tileContainer
        plant.transform.SetParent(tileContainer, worldPositionStays: true);

        // Gán tileContainer làm parent chứa item cho Plant.cs
        Plant plantScript = plant.GetComponent<Plant>();
        plantScript.itemParent = tileContainer;
    }
}

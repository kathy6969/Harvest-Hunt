using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI; // UI Inventory
    private bool isInventoryOpen = false;

    void Start()
    {
        // Tắt Inventory khi bắt đầu game
        isInventoryOpen = false;
        inventoryUI.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
        Time.timeScale = isInventoryOpen ? 0 : 1;
    }

    public bool IsInventoryOpen()
    {
        return isInventoryOpen;
    }
}
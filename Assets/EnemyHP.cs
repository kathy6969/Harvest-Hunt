using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro for text rendering

public class EnemyHP : MonoBehaviour
{
    public float baseHP; // Maximum health points
    private float maxHP; // Maximum health points
    private float currentHP; // Current health points
    [SerializeField] private Image healthBarFill; // Reference to the health bar UI element
    void Start()
    {
        maxHP = baseHP; // Set max HP to base HP
        currentHP = maxHP; // Initialize current HP to max HP
    }
    public void updateHP(float amount)
    {
        currentHP += amount;
        updateHPBar();
    }
    private void updateHPBar()
    {
        float targetFillAmount = currentHP / maxHP;
        healthBarFill.fillAmount = targetFillAmount;
    }
}

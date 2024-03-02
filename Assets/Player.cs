using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int playerLife = 5;
    public TextMeshProUGUI Health; // Reference to the Text UI element
    public GameObject gameOverScreen;

    void Start()
    {
        UpdateHealthUI();
    }

    void Update()
    {
        
    }
    public void DamagePlayer(int damage)
    {
        Debug.Log($"playerLife: {playerLife}");
        playerLife -= damage;
        UpdateHealthUI(); // Call function to update the UI
        if (playerLife <= 0)
        {
            Debug.Log("Game Over");
            ShowGameOverScreen();
        }
    }

    void UpdateHealthUI()
    {
        Health.text = "Health: " + playerLife; // Update the Text UI
    }

    void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
}

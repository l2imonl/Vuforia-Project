using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEconomy : MonoBehaviour
{
    public int money = 100;
    public TextMeshProUGUI Health; // Reference to the Text UI element

    void Start()
    {
        UpdateMoneyUI();
    }

    void Update()
    {
        
    }
    public int GetMoney()
    {
        return money;
    }

    public void ReduceMoney(int towerCost)
    {
        money -= towerCost;
        UpdateMoneyUI(); // Call function to update the UI
        Debug.Log($"money: {money}");
    }

    public void IncreaseMoney(int enemyKilledReward)
    {
        money += enemyKilledReward;
        UpdateMoneyUI(); // Call function to update the UI
        Debug.Log($"money after Kill: {money}");
    }

    void UpdateMoneyUI()
    {
        Health.text = "Money: " + money; // Update the Text UI
    }
}

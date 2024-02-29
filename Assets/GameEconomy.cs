using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEconomy : MonoBehaviour
{
    public int money = 100;

    void Start()
    {
        
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
        Debug.Log($"money: {money}");
    }

    public void IncreaseMoney(int enemyKilledReward)
    {
        money += enemyKilledReward;
        Debug.Log($"money after Kill: {money}");
    }

}

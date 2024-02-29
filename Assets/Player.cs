using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerLife = 5;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void DamagePlayer(int damage)
    {
        Debug.Log($"playerLife: {playerLife}");
        playerLife -= damage;
        if (playerLife <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}

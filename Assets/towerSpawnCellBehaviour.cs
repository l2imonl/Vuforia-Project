using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerSpawnCellBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameEconomy economy;
    public GameObject tower1Prefab;
    
    private bool towerPlaced = false;
    private int towerCost = 30;

    void Start()
    {
        economy = FindObjectOfType<GameEconomy>();
    }

    public void onRaycast(){
        Debug.Log($"Hit:");
        if (!towerPlaced && (economy.GetMoney() - towerCost >= 0)){
            economy.ReduceMoney(towerCost);
            towerPlaced = true;
            // Instantiate the new prefab at the hit position
            // You might want to adjust the position (e.g., slightly above the hit object)
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.01f, 0); // Adjust the 0.1f as needed
            Quaternion spawnRotation = Quaternion.identity; // No rotation, adjust as needed

            Instantiate(tower1Prefab, spawnPosition, spawnRotation);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

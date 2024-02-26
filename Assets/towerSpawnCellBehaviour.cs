using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerSpawnCellBehaviour : MonoBehaviour
{
    public GameObject tower1Prefab;
    
    private bool towerPlaced = false;

    void Start()
    {
        
    }

    public void onRaycast(){
        Debug.Log($"Hit:");
        if(!towerPlaced){
            towerPlaced = true;
            // Instantiate the new prefab at the hit position
            // You might want to adjust the position (e.g., slightly above the hit object)
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.01f, 0); // Adjust the 0.1f as needed
            Quaternion spawnRotation = Quaternion.identity; // No rotation, adjust as needed

            Instantiate(tower1Prefab, spawnPosition, spawnRotation);

            //TODO reduce money
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    private NavigationGrid navigationGrid; // Reference to the Pathfinding component
    private LineRenderer lineRenderer;
    private GameObject nearestEnemy;
    private List<GameObject> enemyList;

    public GameObject projectilePrefab;
    private Transform shootPoint; // Assign a point from where the projectiles are shot
    public float shootInterval = 1f; // Time between shots
    private bool canShoot = true; // To check if the tower can shoot

    // Start is called before the first frame update
    void Start()
    {
        navigationGrid = NavigationGrid.Instance;

        if (navigationGrid != null){
            enemyList = navigationGrid.spawnedPrefabs;
        }

        // Optionally, start shooting automatically when the game starts or when this tower is activated
        StartCoroutine(ShootAtInterval());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShootAtInterval()
    {
        while (true) // Infinite loop to keep shooting at intervals
        {
            if (canShoot)
            {
                ShootEnemy(FindNearestEnemy()); // Implement FindNearestEnemy to find and return the nearest enemy GameObject
                canShoot = false; // Prevent shooting again immediately
                yield return new WaitForSeconds(shootInterval); // Wait for shootInterval seconds
                canShoot = true; // Allow shooting again
            }
            yield return null; // Ensure the loop can yield properly if canShoot is false
        }
    }

    public void ShootEnemy(GameObject nearestEnemy)
    {
        if (nearestEnemy != null && canShoot) // Check if there is an enemy to shoot and if shooting is allowed
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.LookAt(nearestEnemy.transform.position); // Ensure it looks at the enemy
        }
    }

    GameObject FindNearestEnemy()
    {
        float minDistance = 0.24f;

        nearestEnemy = null;

        Vector3 myPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        foreach (GameObject enemy in enemyList)
        {
            float distance = Vector3.Distance(myPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}

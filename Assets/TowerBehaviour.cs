using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public float minDistance = 0.3f;

    private NavigationGrid navigationGrid; // Reference to the Pathfinding component
    private LineRenderer lineRenderer;
    private GameObject nearestEnemy;

    private List<GameObject> enemyList;

    // Start is called before the first frame update
    void Start()
    {
        navigationGrid = NavigationGrid.Instance;

        if (navigationGrid != null){
            enemyList = navigationGrid.spawnedPrefabs;
        }

        // Initialize the LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        FindNearestCubeAndUpdateLine();
    }

    void FindNearestCubeAndUpdateLine()
    {
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

        if (nearestEnemy != null)
        {
            // Draw a line from the reactor GameObject to the nearest cube
            lineRenderer.positionCount = 2;
            
            lineRenderer.SetPosition(0, myPosition);
            lineRenderer.SetPosition(1, nearestEnemy.transform.position);
        }
        else
        {
            // No cubes available, so don't draw a line
            lineRenderer.positionCount = 0;
        }
    }
}

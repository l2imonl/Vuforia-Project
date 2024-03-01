using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Vuforia;

public class StageBuilder : MonoBehaviour
{
    public GameObject ImageTarget1;
    public GameObject ImageTarget2;
    public CornerBehaviour cornerBehaviour1;
    public CornerBehaviour cornerBehaviour2;
    public GameObject wallPrefab;
    public float wallSpacing = 0.1f;
    public GameObject towerSpawnCellPrefab; // Assign a simple prefab for visualization
    private LineRenderer lineRenderer;
    public float gridCellSize = 0.1f; // Desired size for each grid cell (e.g., 0.1 meters)
    private List<GameObject> spawnedPrefabs = new List<GameObject>();

    void Start()
    {
        //initLineRenderer();

        //Vector3 pointA = ImageTarget1.transform.position;
        //Vector3 pointB = ImageTarget2.transform.position;

        //// Calculate the distance between the two points
        //float distance = Vector3.Distance(pointA, pointB);
        //Debug.Log($"Distance: {distance}");

        //// Calculate the number of prefabs needed
        //int numberOfPrefabs = Mathf.FloorToInt(distance / wallSpacing);
        //Debug.Log($"numberOfPrefabs: {numberOfPrefabs}");

        //// Calculate the step size for evenly distributing prefabs
        //float step = 1.0f / numberOfPrefabs;

        //// Instantiate prefabs along the line
        //for (int i = 0; i < numberOfPrefabs; i++)
        //{
        //    // Calculate the position for the current prefab
        //    Vector3 position = Vector3.Lerp(pointA, pointB, step * i);

        //    // Instantiate the prefab at the calculated position
        //    Instantiate(wallPrefab, position, Quaternion.identity);
        //}
    }

    void Update()
    {
        //drawLineRenderer();
    }
    private void initLineRenderer()
    {
        // Initialize the LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.02f; // Change the width of the line here
        lineRenderer.positionCount = 5;

        // Optional: Customize the appearance of the line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = Color.red; // Change line color here
    }

    private void drawLineRenderer()
    {
        // Check if both image targets are active (detected)
        if (cornerBehaviour1.tracked && cornerBehaviour2.tracked)
        {
            Vector3 pointA = ImageTarget1.transform.position;
            Vector3 pointB = ImageTarget2.transform.position;
            Vector3 pointC = new Vector3(pointA.x, pointA.y, pointB.z);
            Vector3 pointD = new Vector3(pointB.x, pointA.y, pointA.z);

            lineRenderer.SetPosition(0, pointA);
            lineRenderer.SetPosition(1, pointD);
            lineRenderer.SetPosition(2, pointB);
            lineRenderer.SetPosition(3, pointC);
            lineRenderer.SetPosition(4, pointA);
        }
        else
        {

        }
    }

    // public Vector3 GenerateRandomPoint(float bias)
    // {
    //     if (cornerBehaviour1.tracked && cornerBehaviour2.tracked)
    //     {
    //         Vector3 pointA = ImageTarget1.transform.position;
    //         Vector3 pointB = ImageTarget2.transform.position;
    //         Vector3 pointC = new Vector3(pointA.x, pointA.y, pointB.z);
    //         Vector3 pointD = new Vector3(pointB.x, pointA.y, pointA.z);

    //         float minX = Mathf.Min(pointA.x, pointB.x, pointC.x, pointD.x);
    //         float maxX = Mathf.Max(pointA.x, pointB.x, pointC.x, pointD.x);
    //         float minZ = Mathf.Min(pointA.z, pointB.z, pointC.z, pointD.z);
    //         float maxZ = Mathf.Max(pointA.z, pointB.z, pointC.z, pointD.z);

    //         float quarterX = (maxX - minX) / 4;
    //         float rangeMinX = bias < 0 ? minX : minX + (3 * quarterX);
    //         float rangeMaxX = bias > 0 ? maxX : maxX - (3 * quarterX);

    //         float randomX = Random.Range(rangeMinX, rangeMaxX);
    //         float randomZ = Random.Range(minZ, maxZ);
    //         float y = pointA.y;

    //         return new Vector3(randomX, y, randomZ);
    //     }
    //     else
    //     {
    //         Debug.Log("ImageTarget not active");
    //         return new Vector3(0,0,0);
    //     }
    // }

    public void CreateRandomPoints(){
        ClearSpawnedObjects(); 

        // GRID
        Vector3 startPoint = ImageTarget1.transform.position;
        Vector3 endPoint = ImageTarget2.transform.position;
        Vector3 lowerLeft = new Vector3(Mathf.Min(startPoint.x, endPoint.x), ImageTarget1.transform.position.y, Mathf.Min(startPoint.z, endPoint.z));

        // Use Vector3.Distance for a positive magnitude and manually calculate for each axis if needed
        float distanceX = Mathf.Abs(endPoint.x - startPoint.x);
        float distanceZ = Mathf.Abs(endPoint.z - startPoint.z);

        // Calculate how many cells fit into the playfield size based on absolute distances
        int gridSizeX = Mathf.FloorToInt(distanceX / gridCellSize);
        int gridSizeY = Mathf.FloorToInt(distanceZ / gridCellSize);

        Debug.Log($"gridSizeX: {gridSizeX}");
        Debug.Log($"gridSizeY: {gridSizeY}");

        void spawnPrefab(Vector3 wallPosition)
        {
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject newWallPrefab = Instantiate(wallPrefab, wallPosition, randomRotation);
            spawnedPrefabs.Add(newWallPrefab);
        }

        for (int x = -1; x <= gridSizeX; x++)
        {
            for (int y = -1; y <= gridSizeY; y++)
            {
                // If Statements to cover the outer Parts of our playing field with a Wall Prefab
                if (x == 0)
                {
                    Vector3 wallPosition = lowerLeft + new Vector3(x * gridCellSize - (0.5f * gridCellSize), 0, y * gridCellSize + gridCellSize);
                    spawnPrefab(wallPosition);
                   
                }
                else if (x == gridSizeX - 1)
                {
                    Vector3 wallPosition = lowerLeft + new Vector3(x * gridCellSize + gridCellSize + gridCellSize + (0.5f * gridCellSize), 0, y * gridCellSize + gridCellSize);
                    spawnPrefab(wallPosition);
                }
                if (y == 0)
                {
                    Vector3 wallPosition = lowerLeft + new Vector3(x * gridCellSize + gridCellSize, 0, y * gridCellSize - (0.5f * gridCellSize));
                    spawnPrefab(wallPosition);
                }
                else if (y == gridSizeY - 1)
                {
                    Vector3 wallPosition = lowerLeft + new Vector3(x * gridCellSize + gridCellSize, 0, y * gridCellSize + gridCellSize + gridCellSize + (0.5f * gridCellSize));
                    spawnPrefab(wallPosition);
                }

                // Grid is bigger due to the surrounding Wall the inside contains the playing field
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                {
                    Vector3 cellPosition = lowerLeft + new Vector3(x * gridCellSize + gridCellSize, 0, y * gridCellSize + gridCellSize);

                    GameObject newGridCellPrefab = Instantiate(towerSpawnCellPrefab, cellPosition, Quaternion.identity);
                    spawnedPrefabs.Add(newGridCellPrefab);  // Keep track of the spawned prefab
                                                            // Debug.Log($"Grid Cell spawned at: {cellPosition}");
                }

            }
        }
    }

    private void ClearSpawnedObjects(){
        foreach (GameObject spawnedPrefab in spawnedPrefabs)
        {
            Destroy(spawnedPrefab);
        }
        spawnedPrefabs.Clear();
    }
}


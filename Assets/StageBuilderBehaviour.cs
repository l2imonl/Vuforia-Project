using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class StageBuilder : MonoBehaviour
{
    public GameObject ImageTarget1;
    public GameObject ImageTarget2;
    public CornerBehaviour cornerBehaviour1;
    public CornerBehaviour cornerBehaviour2;
    public GameObject prefab;
    public GameObject gridCellPrefab; // Assign a simple prefab for visualization

    private GameObject spawnPoint;
    private GameObject endPoint;
    private LineRenderer lineRenderer;

    public float spawnInterval = 2f;
    public float gridCellSize = 0.1f; // Desired size for each grid cell (e.g., 0.1 meters)


    // List to keep track of spawned cubes
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private List<GameObject> spawnedPrefabs = new List<GameObject>();

    void Start()
    {
        // Initialize the LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.02f; // Change the width of the line here
        lineRenderer.positionCount = 5;

        // Optional: Customize the appearance of the line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = Color.red; // Change line color here
    }

    void Update()
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

    public List<GameObject> GetSpawnedList(){
        return spawnedCubes;
    }

    public float GetY(){
        return ImageTarget1.transform.position.y;
    }

    public Vector3 GenerateRandomPoint(float bias)
    {
        if (cornerBehaviour1.tracked && cornerBehaviour2.tracked)
        {
            Vector3 pointA = ImageTarget1.transform.position;
            Vector3 pointB = ImageTarget2.transform.position;
            Vector3 pointC = new Vector3(pointA.x, pointA.y, pointB.z);
            Vector3 pointD = new Vector3(pointB.x, pointA.y, pointA.z);

            float minX = Mathf.Min(pointA.x, pointB.x, pointC.x, pointD.x);
            float maxX = Mathf.Max(pointA.x, pointB.x, pointC.x, pointD.x);
            float minZ = Mathf.Min(pointA.z, pointB.z, pointC.z, pointD.z);
            float maxZ = Mathf.Max(pointA.z, pointB.z, pointC.z, pointD.z);

            float quarterX = (maxX - minX) / 4;
            float rangeMinX = bias < 0 ? minX : minX + (3 * quarterX);
            float rangeMaxX = bias > 0 ? maxX : maxX - (3 * quarterX);

            float randomX = Random.Range(rangeMinX, rangeMaxX);
            float randomZ = Random.Range(minZ, maxZ);
            float y = pointA.y;

            return new Vector3(randomX, y, randomZ);
        }
        else
        {
            Debug.Log("ImageTarget not active");
            return new Vector3(0,0,0);
        }
    }

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

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 cellPosition = lowerLeft + new Vector3(x * gridCellSize + gridCellSize, 0, y * gridCellSize + gridCellSize);
                
                GameObject newGridCellPrefab = Instantiate(gridCellPrefab, cellPosition, Quaternion.identity);
                spawnedPrefabs.Add(newGridCellPrefab);  // Keep track of the spawned prefab
                // Debug.Log($"Grid Cell spawned at: {cellPosition}");
            }
        }

        // Stop any previous spawning coroutine
        StopAllCoroutines();

        // StartCoroutine(SpawnAndMoveCubes(randomPoint1, randomPoint2, spawnInterval, 5f)); // Move each cube over 5 seconds
    }

    private void ClearSpawnedObjects(){
        if (spawnPoint != null) Destroy(spawnPoint);
        if (endPoint != null) Destroy(endPoint);

        // Destroy all previously spawned cubes
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }
        spawnedCubes.Clear();

        foreach (GameObject spawnedPrefab in spawnedPrefabs)
        {
            Destroy(spawnedPrefab);
        }
        spawnedPrefabs.Clear();
    }

    GameObject CreateSphereAtPoint(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f); // Scale down the sphere
        return sphere;
    }

    IEnumerator SpawnAndMoveCubes(Vector3 start, Vector3 end, float interval, float moveDuration)
    {
        while (true) // Infinite loop to keep spawning cubes
        {
            // Spawn a new cube
            GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newCube.transform.position = start;
            newCube.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f); // Scale down the cube

            // Add the new cube to the list
            spawnedCubes.Add(newCube);

            // Start moving the new cube
            StartCoroutine(MoveCube(newCube, start, end, moveDuration));

            // Wait for 'interval' seconds before spawning the next cube
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator MoveCube(GameObject cube, Vector3 start, Vector3 end, float duration)
    {
        // Make sure the cube is at the start position
        cube.transform.position = start;

        // Perform the movement over 'duration' seconds
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            cube.transform.position = Vector3.Lerp(start, end, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the cube has reached the end position
        cube.transform.position = end;

        // Remove the cube from the spawnedCubes list
        spawnedCubes.Remove(cube);

        // Destroy the cube
        Destroy(cube);
    }
}


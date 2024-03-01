using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationGrid : MonoBehaviour
{
    public GameObject marker1; // Assign marker1 in the Inspector
    public GameObject marker2; // Assign marker2 in the Inspector
    public float cellSize = 0.1f; // Desired size for each grid cell
    public List<GameObject> spawnedPrefabs = new List<GameObject>();
    public List<GridCell> pathForEnemys;
    public GameObject enemyPrefab;
    public GameObject spawnPrefab;
    public GameObject endpointPrefab;
    public GameObject groundGrassPrefab;
    public GameObject groundPathPrefab;

    private GridCell[,] grid;
    private int gridSizeX, gridSizeY;
    private GameObject spawn;
    private GameObject endpoint;
    private List<GameObject> groundPrefabs = new List<GameObject>();



    public static NavigationGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    void OnEnable()
    {
        Enemy.OnEnemyDestroyed += RemoveEnemyFromList;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= RemoveEnemyFromList;
    }

    void RemoveEnemyFromList(GameObject enemy)
    {
        if (spawnedPrefabs.Contains(enemy))
        {
            spawnedPrefabs.Remove(enemy);
        }
    }

    public void InitializeGrid()
    {
        clearPrefabs();
        // Calculate the world size of the grid based on markers
        Vector3 worldBottomLeft = new Vector3(Mathf.Min(marker1.transform.position.x, marker2.transform.position.x), marker1.transform.position.y, Mathf.Min(marker1.transform.position.z, marker2.transform.position.z));
        Vector3 worldTopRight = new Vector3(Mathf.Max(marker1.transform.position.x, marker2.transform.position.x), marker1.transform.position.y, Mathf.Max(marker1.transform.position.z, marker2.transform.position.z));
        Vector2 gridWorldSize = new Vector2(Mathf.Abs(worldTopRight.x - worldBottomLeft.x), Mathf.Abs(worldTopRight.z - worldBottomLeft.z));

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / cellSize);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / cellSize);

        CreateGrid(worldBottomLeft, gridWorldSize);

        // init spawn and endpoint prefabs
        Vector3 spawnCoordinates = grid[0, 0].WorldPosition;
        Vector3 endpointCoordinates = grid[gridSizeX - 1, gridSizeY - 1].WorldPosition;


        spawn = Instantiate(spawnPrefab, spawnCoordinates, Quaternion.identity);
        endpoint = Instantiate(endpointPrefab, endpointCoordinates, Quaternion.identity);

        pathForEnemys = FindPath(0,0,gridSizeX-1, gridSizeY-1);
        // create new Path until one can be solved
        while(pathForEnemys.Count == 0){
            CreateGrid(worldBottomLeft, gridWorldSize);
            pathForEnemys = FindPath(0, 0, gridSizeX - 1, gridSizeY - 1);
        }
        visualizePath();
    }

    void CreateGrid(Vector3 worldBottomLeft, Vector2 gridWorldSize)
    {
        grid = new GridCell[gridSizeX, gridSizeY];
        // Adjusted to start from the bottom left of the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * cellSize + cellSize / 2) + Vector3.forward * (y * cellSize + cellSize / 2);
                bool walkable = Random.value >= 0.7f;
                grid[x, y] = new GridCell(walkable, worldPoint, x, y);
            }
        }
    }

    // Optionally, visualize the grid for debugging
    public void visualizePath()
    {
        if(pathForEnemys.Count > 0){
            for (int i = 0; i < pathForEnemys.Count; i++)
            {
                groundPrefabs.Add(Instantiate(groundPathPrefab, pathForEnemys[i].WorldPosition, Quaternion.identity));
            }
        }
    }

    private void clearPrefabs(){
        Destroy(spawn);
        Destroy(endpoint);
        foreach (GameObject spawnedPrefab in spawnedPrefabs)
        {
            Destroy(spawnedPrefab);
        }
        foreach (GameObject spawnedPrefab in groundPrefabs)
        {
            Destroy(spawnedPrefab);
        }
        spawnedPrefabs.Clear();
        groundPrefabs.Clear();
    }

    //Pathfinding
    public List<GridCell> FindPath(int startX, int startY, int targetX, int targetY)
    {
        bool[,] isVisited = new bool[grid.GetLength(0), grid.GetLength(1)];
        Dictionary<GridCell, GridCell> cameFrom = new Dictionary<GridCell, GridCell>();
        Queue<GridCell> queue = new Queue<GridCell>();
        GridCell startNode = grid[startX, startY];
        queue.Enqueue(startNode);
        isVisited[startX, startY] = true;

        while (queue.Count > 0)
        {
            GridCell currentNode = queue.Dequeue();
            if (currentNode.GridX == targetX && currentNode.GridY == targetY)
            {
                return ReconstructPath(cameFrom, currentNode);
            }
            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (!isVisited[neighbor.GridX, neighbor.GridY] && neighbor.IsWalkable)
                {
                    queue.Enqueue(neighbor);
                    isVisited[neighbor.GridX, neighbor.GridY] = true;
                    cameFrom[neighbor] = currentNode;
                }
            }
        }
        // Return an empty path if target is not reachable
        return new List<GridCell>();
    }

    private List<GridCell> GetNeighbors(GridCell node)
    {
        List<GridCell> neighbors = new List<GridCell>();

        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int newX = node.GridX + dx[i];
            int newY = node.GridY + dy[i];

            if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
            {
                neighbors.Add(grid[newX, newY]);
            }
        }
        // Debug.Log($"neighbours.Count: {neighbors.Count}");
        return neighbors;
    }

    private List<GridCell> ReconstructPath(Dictionary<GridCell, GridCell> cameFrom, GridCell current)
    {
        List<GridCell> path = new List<GridCell>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    public void startEnemyCoroutine(){
        StartCoroutine(SpawnPrefabsWithDelay());
    }
    IEnumerator SpawnPrefabsWithDelay()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject gEnemyPrefab = Instantiate(enemyPrefab, new Vector3(grid[0,0].WorldPosition.x, marker1.transform.position.y, grid[0, 0].WorldPosition.z), Quaternion.identity);
            spawnedPrefabs.Add(gEnemyPrefab);
            yield return new WaitForSeconds(2f);
        }
    }
}

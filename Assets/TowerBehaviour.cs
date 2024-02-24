using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    private StageBuilder stageBuilderScript; // Reference to the RandomPointsGenerator
    private LineRenderer lineRenderer;
    private GameObject nearestCube;

    private List<GameObject> enemyList;

    private float y;

    // Start is called before the first frame update
    void Start()
    {
        GameObject stageBuilder = GameObject.FindWithTag("spawner");

        if(stageBuilder != null){
            stageBuilderScript = stageBuilder.GetComponent<StageBuilder>();

            if(stageBuilderScript != null){
                enemyList = stageBuilderScript.GetSpawnedList();
                y = stageBuilderScript.GetY();
            }
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
        float minDistance = float.MaxValue;

        Vector3 myPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        foreach (GameObject cube in enemyList)
        {
            float distance = Vector3.Distance(myPosition, cube.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCube = cube;
            }
        }

        if (nearestCube != null)
        {
            // Draw a line from the reactor GameObject to the nearest cube
            lineRenderer.positionCount = 2;
            
            lineRenderer.SetPosition(0, myPosition);
            lineRenderer.SetPosition(1, nearestCube.transform.position);
        }
        else
        {
            // No cubes available, so don't draw a line
            lineRenderer.positionCount = 0;
        }
    }
}

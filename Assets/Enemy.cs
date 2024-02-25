using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private NavigationGrid navigationGrid; // Reference to the Pathfinding component
    public float speed = 0.5f; // Speed at which the enemy moves along the path

    private List<GridCell> path = new List<GridCell>();
    private int targetIndex;

    void Start()
    {
        navigationGrid = NavigationGrid.Instance;

        path = navigationGrid.pathForEnemys;
        if (path.Count > 0)
        {
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0].WorldPosition;
        targetIndex = 0;

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    // TODO kill enemy reduce player life
                    yield break; // Exit the coroutine if the end of the path is reached
                }
                currentWaypoint = path[targetIndex].WorldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}

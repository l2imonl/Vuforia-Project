using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameEconomy economy;
    public Player player;

    public float speed = 0.3f; // Speed at which the enemy moves along the path
    public int health = 100;
    public int enemyKilledReward = 10;
    public int damageToPlayer = 1;
    public delegate void EnemyDestroyed(GameObject enemy);
    public static event EnemyDestroyed OnEnemyDestroyed;

    private NavigationGrid navigationGrid; // Reference to the Pathfinding component
    private List<GridCell> path = new List<GridCell>();
    private int targetIndex;

    void Start()
    {
        economy = FindObjectOfType<GameEconomy>();
        player = FindObjectOfType<Player>();

        navigationGrid = NavigationGrid.Instance;

        path = navigationGrid.pathForEnemys;
        if (path.Count > 0)
        {
            StartCoroutine(FollowPath());
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"TakeDamage: {damage}");
        health -= damage;
        if (health <= 0)
        {
            OnEnemyDestroyed?.Invoke(gameObject);
            Destroy(gameObject); // Destroy enemy if health is 0 or less

            economy.IncreaseMoney(enemyKilledReward);
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
                    OnEnemyDestroyed?.Invoke(gameObject);
                    Destroy(gameObject);
                    player.DamagePlayer(damageToPlayer);
                    yield break; // Exit the coroutine if the end of the path is reached
                }
                currentWaypoint = path[targetIndex].WorldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}

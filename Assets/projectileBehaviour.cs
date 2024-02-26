using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehaviour : MonoBehaviour
{
    public float speed = 1f;
    public int damage = 20;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            // Assume the enemy has a script with a method 'TakeDamage'
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject); // Destroy the projectile on hit
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}

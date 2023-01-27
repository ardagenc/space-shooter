using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject explosion;
    private SpawnManager spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            
            CircleCollider2D asteroidCollider = GetComponent<CircleCollider2D>();

            //enemy Spawn after asteroid explodes
            spawnManager.StartSpawning();

            asteroidCollider.enabled = false;
            Destroy(other.gameObject);
            Destroy(gameObject, 0.25f);
        }
    }
}

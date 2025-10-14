using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;

    private float spawnX = 40f;  // Where obstacles spawn
    public float groundY = 0f; // Ground height
    public float topY = 22f; // Top height
    public float randomOffset = 2f; // Variation near edges

    private float startDelay = 2;
    private float repeatRate = 2;
    
    private PlayerController playerControllerScript;

    
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    
    }

    void Update()
    {
        
    }

    void SpawnObstacle ()
    {
       // if (playerControllerScript.gameOver == false)
       // {
       //     Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
       // }

       // Random choose top or bottom
       bool spawnFromTop = Random.value > 0.5f;

       float yPos;

       if (spawnFromTop) {
        // Spawn near the top
        yPos = topY - Random.Range(0f, randomOffset);
       } else
       {
        // Spawn near the ground
        yPos = groundY + Random.Range(0f, randomOffset);
       }

       Vector3 spawnPos = new Vector3(spawnX, yPos, 0);

       Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);

    }
}
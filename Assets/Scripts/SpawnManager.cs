using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject powerUpPrefab;

    private float spawnX = 40f;  // Where obstacles spawn
    public float groundY = 0f; // Ground height
    public float topY = 22f; // Top height
    public float randomOffset = 5f; // Variation near edges ??

    private float startDelay = 2f; 
    private float repeatRate = 2f; 

    private float powerUpSpawnRate = 8f;

    private float difficultyTimer = 0f;
    private float difficultyInterval = 10f; // Every 10 seconds
    private float minRepeatRate = 0.5f;
    
    private PlayerController playerControllerScript;
    private GameManager gameManager;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnPowerUp", startDelay + 1f, powerUpSpawnRate);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    
    }

    void Update()
    {
        if (!playerControllerScript.gameOver && gameManager.isGameActive)
        {
        difficultyTimer += Time.deltaTime;

        if (difficultyTimer >= difficultyInterval && repeatRate > minRepeatRate)
        {
            repeatRate -= 0.2f;
            difficultyTimer = 0f;

            CancelInvoke("SpawnObstacle");
            InvokeRepeating("SpawnObstacle", 0f, repeatRate);
        }
        }
        
    }

    void SpawnObstacle ()
    {
       if (!playerControllerScript.gameOver && gameManager.isGameActive)
       {
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

    void SpawnPowerUp() {
        if (!playerControllerScript.gameOver && gameManager.isGameActive)
        {
        float randomY = Random.Range(groundY + 1f, topY - 1f);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

        Instantiate(powerUpPrefab, spawnPos, Quaternion.identity); // ??
        }
    }
}


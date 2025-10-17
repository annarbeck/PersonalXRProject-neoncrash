using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    // References
    private PlayerController playerController;
    private GameManager gameManager;

    // Prefabs to spawn
    public GameObject obstaclePrefab;
    public GameObject powerUpPrefab;

    // Spawn Position settings
    private float spawnX = 40f;
    public float groundY = 0f;
    public float topY = 22f;
    public float randomOffset = 4f;

    // Spawn timing
    private float obstacleSpawnRate = 2f;
    private float powerUpSpawnRate = 8f;
    private float obstacleSpawnTimer = 0f;
    private float powerUpSpawnTimer = 0f;

    // Difficulty scaling
    private float difficultyInterval = 10f; // Every 10 seconds
    private float minObstacleSpawnRate = 0.5f;
    private float difficultyTimer = 0f;

    void Start()
    {
        // Get references
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Only spawn if game is active and not over
        if (!playerController.gameOver && gameManager.isGameActive)
        {
            HandleObstacleSpawing();
            HandlePowerUpSpawning();
            HandleDifficultyScaling();
        }
    }


    private void HandleObstacleSpawing()
    {
        obstacleSpawnTimer += Time.deltaTime;
        if (obstacleSpawnTimer >= obstacleSpawnRate)
        {
            SpawnObstacle();
            obstacleSpawnTimer = 0f;
        }
    }

    private void HandlePowerUpSpawning()
    {
        powerUpSpawnTimer += Time.deltaTime;
        if (powerUpSpawnTimer >= powerUpSpawnRate)
        {
            SpawnPowerUp();
            powerUpSpawnTimer = 0f;
        }
    }
    
    private void HandleDifficultyScaling()
    {
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyInterval && obstacleSpawnRate > minObstacleSpawnRate)
        {
            obstacleSpawnRate -= 0.2f;
            difficultyTimer = 0f;
        }
    }
        

    void SpawnObstacle ()
    {
         // Random choose top or bottom spawn
        bool spawnFromTop = Random.value > 0.5f;
        float yPos = spawnFromTop ? topY - Random.Range(0f, randomOffset) : groundY + Random.Range(0f, randomOffset);

        Vector3 spawnPos = new Vector3(spawnX, yPos, 0);
        Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
    }

    void SpawnPowerUp() 
    {
        float randomY = Random.Range(groundY + 1f, topY - 1f);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

        Instantiate(powerUpPrefab, spawnPos, Quaternion.identity); 
    }
}


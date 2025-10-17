using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // References
    private PlayerController playerController;
    private GameManager gameManager;

    // Movement settings
    private float speed = 30;
    private float leftBound = -20;

    // Scoring settings
    private int scoreValue = 1;
    private bool hasScored = false;


    void Start()
    {
        // Get references
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Move left if game is active and not over
        if (playerController.gameOver == false && gameManager.isGameActive == true)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        // Score when obstacle passes player and hasn't scored yet
        if (gameObject.CompareTag("Obstacle") && !hasScored && transform.position.x < playerController.transform.position.x) 
        {
            hasScored = true;
            gameManager.UpdateScore(scoreValue); 
        }

        // Destroy obstacles and powerups that go off-screen
        if (transform.position.x < leftBound) //&& gameObject.CompareTag("Obstacle"))
        {
            if (gameObject.CompareTag("Obstacle") || gameObject.CompareTag("Powerup"))
            {
                Destroy(gameObject);
            }
        }
        
    }

}

using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Movement variables
    private float speed = 30;
    private float leftBound = -20;

    private PlayerController playerController;
    private GameManager gameManager;

    private int pointValue = 1;
    private bool scored = false; // prevent multiple scoring


    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Move only if the game is not over
        if (playerController.gameOver == false && gameManager.isGameActive == true)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (gameObject.CompareTag("Obstacle") && !scored && transform.position.x < playerController.transform.position.x) {
            scored = true; // mark as scored
            gameManager.UpdateScore(pointValue); // Increase score by 1
        }

        // Destroy obstacles and powerups that go off-screen
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Powerup")) {
            Destroy(gameObject);
        }
        
    }

}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    //private GameManager gameManager;

    //Movement settings
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public float quickDropForce = 13f;

    public bool isInvincible = false;
    public float powerUpDuration = 5f;


    public bool gameOver = false;
        
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerRb.useGravity = true;

        // Freeze everything except Y movement
        playerRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        //Up Input
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            playerRb.linearVelocity = new Vector3(0, 0, 0); //playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //Quickly down
        if (Input.GetKeyDown(KeyCode.DownArrow) && !gameOver)
        {
            playerRb.AddForce(Vector3.down * quickDropForce, ForceMode.Impulse);
        }
        
        if (transform.position.y > 22f) 
        {
            Debug.Log("Game Over!");
            gameOver = true;
        }
 
    }

    private void OnCollisionEnter(Collision collision) 
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
        } 
            
        if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            gameOver = true;
            Debug.Log("Game Over!");
        } else {
            Debug.Log("Obstacle hit ignored (invincible!)");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            StartCoroutine(ActivatePowerUp());
            
        }
    }

    private IEnumerator ActivatePowerUp()
    {
        isInvincible = true;
        Debug.Log("Power-Up Activated");

        // Add visual feedback here (color, sounds, glow etc.)

        yield return new WaitForSeconds(powerUpDuration);

        isInvincible = false;
        Debug.Log("Power-Up Ended");
    }
}

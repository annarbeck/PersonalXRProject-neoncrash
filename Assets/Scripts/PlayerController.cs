using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    // References
    private Rigidbody playerRb;
    private GameManager gameManager;
    private AudioSource audioSource;
    public InputActionReference startActionReference; 
    public InputActionReference jumpActionReference; 
    public InputActionReference dropActionReference;

    // Visual and Audio effects
    public ParticleSystem explosionParticle;
    public ParticleSystem playerParticle;
    public AudioClip crashSound;
    public AudioClip powerupSound;

    //Movement settings
    public float jumpForce = 12f;
    public float quickDropForce = 13f;

    // Power-up settings
    public bool isInvincible = false;
    public float powerUpDuration = 5f;
    public GameObject Powerup;
    public Slider powerupProgressBar;

    // Crash feedback setting
    public float shrinkDuration = 0.1f;
    public float cameraShakeIntensity = 0.2f;
    public float cameraShakeDuration = 0.2f;

    // Game state
    public bool gameOver = false;

    void Start()
    {
        // Get references
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        // Disable gravity until game starts
        playerRb.useGravity = false;
    }

    void Update()
    {
        // Start game on spacebar press
        if (!gameManager.isGameActive && startActionReference.action.triggered) // Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.StartGame();
            playerRb.useGravity = true;
            playerParticle.Play();
        }

        // Handle player input during game
        if (gameManager.isGameActive && !gameOver)
        {
            if(jumpActionReference.action.triggered) // if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                SendHaptics(0.3f, 0.05f); 
            }

            if(dropActionReference.action.triggered) //if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerRb.AddForce(Vector3.down * quickDropForce, ForceMode.Impulse);
                SendHaptics(0.2f, 0.05f);
            }

            // Game over if the player flies to the top of the screen
            if (transform.position.y > 22f)
            {
                Debug.Log("Game Over!");
                StartCoroutine(DeathSequence());
                gameOver = true;
            }
        }
    }
    

    private void OnCollisionEnter(Collision collision) 
    {
        // End game if player hits ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameOver = true;
            StartCoroutine(DeathSequence());
            Debug.Log("Game Over!");
        } 

        // End game if player hits obstacles and does not have an activated powerup    
        if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            gameOver = true;
            StartCoroutine(DeathSequence());
            Debug.Log("Game Over!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Activate powerup on trigger
        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            StartCoroutine(ActivatePowerUp());
        }
    }

    private IEnumerator ActivatePowerUp()
    {
        isInvincible = true;
        audioSource.PlayOneShot(powerupSound);
        SendHaptics(0.5f, 0.1f);
        Debug.Log("Power-Up Activated");

        Powerup.SetActive(true);
        powerupProgressBar.gameObject.SetActive(true);

        float elapsed = 0f;

        // Update progressbar over time
        while (elapsed < powerUpDuration)
        {
            elapsed += Time.deltaTime;
            powerupProgressBar.value = 1f - (elapsed / powerUpDuration);
            yield return null;
        }

        // End powerup
        powerupProgressBar.gameObject.SetActive(false);
        isInvincible = false;
        Powerup.SetActive(false);
        Debug.Log("Power-Up Ended");
    }



    IEnumerator DeathSequence()
    {
        // Stop particle effect on the player
        playerParticle.Stop();

        // Disable player control
        this.enabled = false;

        // Freeze the player
        playerRb.linearVelocity = Vector3.zero;
        playerRb.useGravity = false;

        // Shrink player 
        Vector3 startScale = transform.localScale;
        float t = 0f;
        while (t < shrinkDuration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / shrinkDuration);
            yield return null;
        }

        // Explosion and sound
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        audioSource.PlayOneShot(crashSound);
        SendHaptics(0.8f, 0.2f);
        Debug.Log("Crash");

        yield return new WaitForSeconds(1f);

        gameManager.GameOver();
    }

    private void SendHaptics(float amplitude, float duration) {
        // Try both controllers (left and right) 
        var left = GameObject.Find("LeftHand Controller")?.GetComponent<ActionBasedController>();
        var right = GameObject.Find("RightHand Controller")?.GetComponent<ActionBasedController>();

        if (left != null)
            left.SendHapticImpulse(amplitude, duration); 
        
        if (right != null) 
            right.SendHapticImpulse(amplitude, duration); }

}

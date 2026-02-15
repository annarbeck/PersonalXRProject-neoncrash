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
    public AudioSource startOrbAudio;

    public InputActionReference jumpActionReference; 
    public InputActionReference dropActionReference;

    // Visual and Audio effects
    public ParticleSystem explosionParticle;
    public ParticleSystem playerParticle;
    public AudioClip crashSound;
    public AudioClip powerupSound;
    public AudioClip startSound;

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

    // Game state
    public bool gameOver = false;

    public GameObject startOrb; // drag your orb here 
    public float startDelay = 0.5f; // delay before game starts 
    public float fadeDuration = 0.5f;

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
        // Handle player input during game
        if (gameManager.isGameActive && !gameOver)
        {
            if (jumpActionReference.action.triggered)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            if (dropActionReference.action.triggered)
            {
                playerRb.AddForce(Vector3.down * quickDropForce, ForceMode.Impulse);
            }

            // Game over if the player flies to the top of the screen
            if (transform.position.y > 22f)
            {
                Debug.Log("Game Over!");
                StartCoroutine(DeathSequence());
                gameOver = true;
            }
        }

        if (gameManager.isPaused)
            return;

    }

    public void ActivateStartOrb()
    {
        if (!gameManager.isGameActive)
        {
            startOrbAudio.clip = startSound;

            startOrbAudio.pitch = 0.8f;
            startOrbAudio.volume = 0.5f;

            startOrbAudio.Play();

            StartCoroutine(FadeAndStart());
        }
    }

    
    private IEnumerator FadeAndStart()
{
    // Shrink the orb instead of fading
    Vector3 startScale = startOrb.transform.localScale;
    float t = 0f;

    while (t < fadeDuration)
    {
        t += Time.deltaTime;
        startOrb.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / fadeDuration);
        yield return null;
    }

    // Disable orb
    startOrb.SetActive(false);

    // Wait before starting game
    yield return new WaitForSeconds(startDelay);

    // Start game
    gameManager.StartGame();
    playerRb.useGravity = true;
    playerParticle.Play();
}



    private void OnCollisionEnter(Collision collision) 
    {
        // End game if player hits ground
        if (collision.gameObject.CompareTag("CubeEnvironment"))
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

    public IEnumerator ActivatePowerUp()
    {
        isInvincible = true;
        audioSource.PlayOneShot(powerupSound);
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
        FindAnyObjectByType<WebSocketClient>()?.SendCrash();
        Debug.Log("Crash");

        yield return new WaitForSeconds(1f);

        gameManager.GameOver();
    }

}

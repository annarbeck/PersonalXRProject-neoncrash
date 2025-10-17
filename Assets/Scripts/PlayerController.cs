using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;
    public ParticleSystem playerParticle;
    public AudioClip crashSound;
    public Camera mainCamera;

    //Movement settings
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public float quickDropForce = 13f;

    // Powerup settings
    public bool isInvincible = false;
    public float powerUpDuration = 5f;
    public GameObject Powerup;
    public Slider powerupProgressBar;
    public AudioClip powerupSound;

    // Crash feedback setting
    public float shrinkDuration = 0.1f;
    public float cameraShakeIntensity = 0.2f;
    public float cameraShakeDuration = 0.2f;
    public float slowMoDuration = 0.15f;
    public float slowMoScale = 0.5f;

    private AudioSource audioSource;

    public bool gameOver = false;
        
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        playerRb.useGravity = false;

        // Freeze everything except Y movement
        playerRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Start the game
        if (!gameManager.isGameActive && Input.GetKeyDown(KeyCode.Space))
        {
        gameManager.StartGame();
            playerRb.useGravity = true;
            playerParticle.Play();
        }

        if (gameManager.isGameActive && !gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
            {
            playerRb.linearVelocity = new Vector3(0, 0, 0); 
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
            StartCoroutine(DeathSequence());
            gameOver = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            gameOver = true;
            StartCoroutine(DeathSequence());
            Debug.Log("Game Over!");
        } 
            
        if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            gameOver = true;
            StartCoroutine(DeathSequence());
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
        audioSource.PlayOneShot(powerupSound);
        Debug.Log("Power-Up Activated");
        Powerup.SetActive(true);
        powerupProgressBar.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < powerUpDuration)
        {
            elapsed += Time.deltaTime;
            powerupProgressBar.value = 1f - (elapsed / powerUpDuration);
            yield return null;
        }

        powerupProgressBar.gameObject.SetActive(false);
        isInvincible = false;
        Powerup.SetActive(false);
        Debug.Log("Power-Up Ended");
    }



    IEnumerator DeathSequence()
    {
        // Stop flying particles
        playerParticle.Stop();

        // Disable player control
        this.enabled = false;

        // Keeps the player object in place after explosion
        playerRb.linearVelocity = Vector3.zero;
        playerRb.useGravity = false;

        // Makes the ball disappear when exploding 
        Vector3 startScale = transform.localScale;
        float t = 0f;
        while (t < shrinkDuration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t / shrinkDuration);
            yield return null;
        }

        // Explosion Particles
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

        // Crash sound
        audioSource.PlayOneShot(crashSound);
        Debug.Log("Crash Sound");

        // Camera shake
        StartCoroutine(ShakeCamera(cameraShakeDuration, cameraShakeIntensity));

        yield return new WaitForSeconds(1f);

        gameManager.GameOver();
    }

    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    //Movement settings
    public float jumpForce = 7f;
    public float fallMultiplier = 2f;
    public float quickDropForce = 10f;
    public float maxFallSpeed = -15f;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        playerRb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Jump up
        if (Input.GetKeyDown(KeyCode.Space)) //&& !gameOver
        {
            playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // playerAnim.SetTrigger("Jump_trig");
        }

        //Quickly down
        if (Input.GetKeyDown(KeyCode.DownArrow)) // && !gameOver
        {
            playerRb.AddForce(Vector3.down * quickDropForce, ForceMode.Impulse);
        }
        
        if (playerRb.linearVelocity.y < maxFallSpeed)
        {
            playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, maxFallSpeed, playerRb.linearVelocity.z);
        }
    }
}

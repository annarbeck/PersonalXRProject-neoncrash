using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.UI;

public class StartOrb : MonoBehaviour
{
    private GameManager gameManager;
    //public ParticleSystem burstEffect;
    //public AudioSource popSound;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.localPosition += Vector3.up * Mathf.Sin(Time.time * 2f) * 0.0005f; 
        transform.Rotate(0, 30f * Time.deltaTime, 0);
    }

        public void OnOrbActivated()
    {
        if (gameManager.isGameActive)
            return;

        // Optional effects
        //if (burstEffect != null) burstEffect.Play();
        //if (popSound != null) popSound.Play();

        // Hide the orb
        gameObject.SetActive(false);

        // Start the game
        gameManager.StartGame();
    }
}





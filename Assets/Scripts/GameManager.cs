using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // UI elements
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject pauseMenu; // Assign your pause panel here 
    public Button resumeButton; // Optional: hook up a resume button

    // Game state
    public bool isGameActive;
    public bool isPaused; 

    // Score tracking
    private int score = 0;
    private int highScore = 0;

    void Start()
    { 
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();

        // When i want to reset my highscore:
        // PlayerPrefs.DeleteKey("HighScore");

        // Hide pause menu at start 
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void StartGame() 
    {
        isGameActive = true;
        score = 0;
        
        UpdateScore(0);
        scoreText.gameObject.SetActive(true);
    }

    public void UpdateScore(int scoreToAdd) 
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;

        // Update high score if needed
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText() 
    {
        highScoreText.text = "High Score: " + highScore;
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);

        // Delay showing restart button
        Invoke("ShowRestartButton", 1.5f);
    }

    private void ShowRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        // Reload scene 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // PAUSE SYSTEM  
    public void PauseGame()
    {
        if (!isGameActive || isPaused)
            return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        Debug.Log("Game Paused");
    } 
        
        public void ResumeGame() {
        if (!isPaused)
            return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
            
        Debug.Log("Game Resumed"); 
            }
}

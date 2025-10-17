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
    public GameObject titleScreen;

    // Game state
    public bool isGameActive;

    // Score tracking
    private int score = 0;
    private int highScore = 0;

    void Start()
    { 
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();

        // When i want to reset my highscore:
        // PlayerPrefs.DeleteKey("HighScore");
    }

    public void StartGame() 
    {
        isGameActive = true;
        score = 0;
        
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
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
}

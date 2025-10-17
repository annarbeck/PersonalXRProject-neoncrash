using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;

    public bool isGameActive;

    private int score = 0;
    private int highScore = 0;

    void Start()
    {
        // PlayerPrefs.DeleteKey("HighScore"); // When I want to reset my highscore  
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        isGameActive = true;
        
        UpdateScore(0);

        titleScreen.gameObject.SetActive(false);
        
    }

    public void UpdateScore(int scoreToAdd) {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText() {
        if (highScoreText != null) {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    public void GameOver() {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;

        Invoke("ShowRestartButton", 1.5f);
    }

    private void ShowRestartButton()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

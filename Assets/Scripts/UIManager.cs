using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gamePanel;
    public GameObject gameOverScreen;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    // public Image powerUpActiveSprite;
    // public Image powerUpSprite;
    
    // power ups
    // public Image shieldImage;
    // public Image speedUpImage;
    // public Image scoreBoostImage;

    // Player 1 Power Up UI
    [Header("Player PowerUps")]
    public Image[] p1Images;

    public void PlaySound()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void PlaySinglePlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void PlayMultiPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void Quit()
    {
        Application.Quit();
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SoundManager.Instance.Play(Sounds.ButtonClick);
    }

    public void ShowGameOverScreen(int score)
    {
        Time.timeScale = 0f;
        gamePanel.SetActive(false);
        gameOverScreen.SetActive(true);
        gameOverScoreText.text = "Your Score : " + score;
    }

    public void ShowGameOverScreen(SnakeController.PlayerNumber playerNumber)
    {
        Time.timeScale = 0f;
        gamePanel.SetActive(false);
        gameOverScreen.SetActive(true);
        if((int)playerNumber == 0)
            gameOverScoreText.text = "Player 2 Won";
        else
            gameOverScoreText.text = "Player 2 Won";
            
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score : " + score;
    }

    public IEnumerator UpdatePowerUp(PowerUp.PowerUpType powerUp, float duration)
    {
        // p1Images[(int)powerUp].color = Color.green;
        // yield return new WaitForSeconds(duration);
        // p1Images[(int)powerUp].color = Color.white;    

        Color color = p1Images[(int)powerUp].color;
        color.a = 1f;
        p1Images[(int)powerUp].color = color;

        yield return new WaitForSeconds(duration);

        color.a = 0.5f;
        p1Images[(int)powerUp].color = color;

    }

}

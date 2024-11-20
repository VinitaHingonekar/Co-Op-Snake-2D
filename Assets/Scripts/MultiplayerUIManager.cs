using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiplayerUIManager : MonoBehaviour
{
    [Header("Score Texts")]
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    // Player 1 Power Up UI
    [Header("Player 1 PowerUps")]
    public Image[] p1Images;

    // Player 2 Power Up UI
    [Header("Player 2 PowerUps")]
    public Image[] p2Images;

    public void UpdateScore(int score, SnakeController.PlayerNumber playerNumber)
    {
        if((int)playerNumber == 0)
        {
            player1ScoreText.text = "Score : " + score;
        }
        else
        {
            player2ScoreText.text = "Score : " + score;
        }
    }

    public IEnumerator UpdatePowerUp(PowerUp.PowerUpType powerUp, SnakeController.PlayerNumber playerNumber, float duration)
    {
        if((int)playerNumber == 0)
        {
            Color color = p1Images[(int)powerUp].color;
            color.a = 1f;
            p1Images[(int)powerUp].color = color;

            yield return new WaitForSeconds(duration);

            color.a = 0.5f;
            p1Images[(int)powerUp].color = color;
        }
        else
        {
            Color color = p2Images[(int)powerUp].color;
            color.a = 1f;
            p2Images[(int)powerUp].color = color;

            yield return new WaitForSeconds(duration);

            color.a = 0.5f;
            p2Images[(int)powerUp].color = color;

        }
    }

}

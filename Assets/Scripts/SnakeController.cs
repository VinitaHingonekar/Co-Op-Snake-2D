using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(20, 20);

    private Vector2Int headPosition;
    private Vector2Int moveDirection;

    private float moveTimer;
    private float moveTimerMax;

    private FoodSpawner foodSpawner;
    
    private List<Transform> snakeBodyParts = new List<Transform>();
    [Header("Snake Body Prefabs")]
    [SerializeField] private GameObject p1BodyPartPrefab;
    [SerializeField] private GameObject p2BodyPartPrefab;

    private int decreaseLenght = 2;

    // score variables
    private int score;
    private int foodScore = 10;

    // power up varaibles
    private int scoreMultiplier = 1;
    private float speedMultiplier = 1f;
    private float cooldownTimer = 3f;
    private bool isOnCooldown = false;
    private bool hasShield = false;
    private bool canUsePowerUp = true;

    // UI
    [Header("UI Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private MultiplayerUIManager multiplayerUIManager;

    // Multiplayer
    public enum PlayerNumber { PlayerOne, PlayerTwo};

    [Header("Multiplayer")]
    [SerializeField] private SnakeController[] snakes;
    public PlayerNumber playerNumber;
    public bool isMultiPlayerScene;

    private void Awake()
    {
        // headPosition = new Vector2Int(10, 10);
        moveTimerMax = 0.1f;
        moveTimer = moveTimerMax;
        moveDirection = Vector2Int.right;
    }

    private void Start()
    {
        foodSpawner = FindObjectOfType<FoodSpawner>();
        if(isMultiPlayerScene)
        {
            if((int)playerNumber == 0)
                headPosition = new Vector2Int(10, 15);
            else
                headPosition = new Vector2Int(10, 5);
        }
        else
            headPosition = new Vector2Int(10, 10);
    }

    void Update()
    {
        HandleInput();
        Move();

    }

    private void HandleInput()
    {
        if(playerNumber == PlayerNumber.PlayerOne)
        {
            if(Input.GetKeyDown(KeyCode.W) && moveDirection != Vector2Int.down)
                moveDirection = Vector2Int.up;
            else if(Input.GetKeyDown(KeyCode.S) && moveDirection != Vector2Int.up)
            moveDirection =  Vector2Int.down;
            else if(Input.GetKeyDown(KeyCode.A) && moveDirection != Vector2Int.right)
                moveDirection = Vector2Int.left;
            else if(Input.GetKeyDown(KeyCode.D) && moveDirection != Vector2Int.left)
                moveDirection = Vector2Int.right;
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2Int.down)
                moveDirection = Vector2Int.up;
            else if(Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2Int.up)
            moveDirection =  Vector2Int.down;
            else if(Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2Int.right)
                moveDirection = Vector2Int.left;
            else if(Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2Int.left)
                moveDirection = Vector2Int.right;
        }
    }

    private void Move()
    {
        moveTimerMax = 0.1f / speedMultiplier;

        moveTimer += Time.deltaTime;
        if(moveTimer >= moveTimerMax)
        {
            moveTimer -= moveTimerMax;

            // moving the body aorts
            for ( int i = snakeBodyParts.Count -1; i >0; i--)
            {
                snakeBodyParts[i].position = snakeBodyParts[i-1].position;
            }

            if ( snakeBodyParts.Count > 0)
            {
                snakeBodyParts[0].position = transform.position;
            }

            // moving the head
            headPosition += moveDirection;
            
            // screen wrapping 
            headPosition = WrapGridPosition(headPosition);

            transform.position = new Vector3(headPosition.x, headPosition.y);
            transform.eulerAngles = new Vector3 (0, 0, GetAngleFromDirection(moveDirection));

            // checking collision with body
            if(IsOccupiedBySnake(headPosition))
            {
                Death();
            }
        }
    }

    private Vector2Int WrapGridPosition(Vector2Int position)
    {
        position.x = (position.x + gridSize.x) % gridSize.x;
        position.y = (position.y + gridSize.y) % gridSize.y;
        return position;
    }

    private float GetAngleFromDirection(Vector2Int direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(angle < 0) angle += 360;
        return angle;
    }

    public bool IsOccupiedBySnake(Vector2Int position)
    {
        // if (headPosition == position)
        //     return true;
        
        foreach(SnakeController snake in snakes)
        {
            foreach(Transform bodyPart in snake.snakeBodyParts)
            {
                if(Vector2Int.RoundToInt(bodyPart.position) == position)
                    return true;
            }
        }
        return false;
    }

    private void Death()
    {
        Debug.Log(this.name + "dies");
        if(hasShield)
        {
            Debug.Log("Shield used");
            hasShield = false;
        }
        else
        {
            if(isMultiPlayerScene)
            {
                uiManager.ShowGameOverScreen(playerNumber);
            }
            else
            {
                uiManager.ShowGameOverScreen(score);
            }
            score = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("MassGainer"))
        {
            Destroy(other.gameObject);
            Grow();
            AddScore(foodScore);
        }
        else if(other.CompareTag("MassBurner"))
        {
            Destroy(other.gameObject);
            Shrink(decreaseLenght);
            AddScore(-foodScore);
        }
        else if(other.CompareTag("PowerUp"))
        {
            if(canUsePowerUp)
            {
                if(!isOnCooldown)
                {
                    PowerUp powerUp = other.GetComponent<PowerUp>();
                    Destroy(other.gameObject);
                    ActivatePowerUp(powerUp.powerUpType, powerUp.duration);
                    StartCoroutine(StartCoolDownTimer());
                }
                else
                {
                    Debug.Log("Player on Cool Down");
                }
            }
            else
            {
                Debug.Log("Player cannot use Power Up");
            }
        }
    }

    private void Grow()
    {
        Vector3 newBodyPartPosition = (snakeBodyParts.Count == 0) 
            ? transform.position // spawn at head
            : snakeBodyParts[snakeBodyParts.Count - 1].position; //  spawn at the last position

        GameObject bodyPrefab;

        if(isMultiPlayerScene)
            bodyPrefab = (int)playerNumber == 0 ? p1BodyPartPrefab : p2BodyPartPrefab;
        else
            bodyPrefab = p1BodyPartPrefab;

        GameObject newBodyPart = Instantiate(bodyPrefab, newBodyPartPosition, Quaternion.identity);
        snakeBodyParts.Add(newBodyPart.transform);
    }

    private void Shrink(int lenght)
    {
        for(int i=0; i < lenght; i++)
        {
            if (snakeBodyParts.Count > 0)
            {
                Transform lastPart = snakeBodyParts[snakeBodyParts.Count - 1];
                snakeBodyParts.Remove(lastPart);
                Destroy(lastPart.gameObject);
            }
            else
            {
                Debug.Log("Cannot Shrink");
                break;
            }
        }
    }

    private void AddScore(int points)
    {
        score += points * scoreMultiplier;
        Debug.Log("Score: " + score);
        if(isMultiPlayerScene)
            multiplayerUIManager.UpdateScore(score, playerNumber);
        else
            uiManager.UpdateScore(score);
    }

    private void ActivatePowerUp(PowerUp.PowerUpType powerUpType, float duration)
    {
        canUsePowerUp = false;
        if(isMultiPlayerScene)
            StartCoroutine(multiplayerUIManager.UpdatePowerUp(powerUpType, playerNumber, duration));
        else    
            StartCoroutine(uiManager.UpdatePowerUp(powerUpType, duration));

        switch (powerUpType)
        {
            case PowerUp.PowerUpType.Shield:
                StartCoroutine(ActivateShield(duration));
                break;
            case PowerUp.PowerUpType.SpeedUp:
                StartCoroutine(ActivateSpeedUp(duration));
                break;
            case PowerUp.PowerUpType.ScoreBoost:
                StartCoroutine(ActivateScoreBoost(duration));
                break;
        }
    }

    IEnumerator StartCoolDownTimer()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        isOnCooldown = false;
    }

    IEnumerator ActivateShield(float duration)
    {
        hasShield = true;
        yield return new WaitForSeconds(duration);
        hasShield = false;
        canUsePowerUp = true;
        // if(!isMultiPlayerScene)
        //     uiManager.ResetPowerUpUI();
        // else
        //     multiplayerUIManager.ResetPowerUpUI(playerNumber);
    }

    IEnumerator ActivateSpeedUp(float duration)
    {
        speedMultiplier = 1.5f;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
        canUsePowerUp = true;
        // if(!isMultiPlayerScene)
        //     uiManager.ResetPowerUpUI();
        // else
        //     multiplayerUIManager.ResetPowerUpUI(playerNumber);
    }

    IEnumerator ActivateScoreBoost(float duration)
    {
        scoreMultiplier = 2;
        yield return new WaitForSeconds(duration);
        scoreMultiplier = 1;
        canUsePowerUp = true;
        // if(!isMultiPlayerScene)
        //     uiManager.ResetPowerUpUI();
        // else
        //     multiplayerUIManager.ResetPowerUpUI(playerNumber);
    }

    public Vector2Int GetSnakeHeadPosition()
    {
        return headPosition;
    }

    public List<Transform> GetSnakeBodyParts()
    {
        return snakeBodyParts;
    }

    public int GetSnakeSize()
    {
        return 1+ snakeBodyParts.Count;
    }


}
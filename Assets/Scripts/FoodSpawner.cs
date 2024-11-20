using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject massGainerPrefab;
    public GameObject massBurnerPrefab;
    public GameObject[] powerUpPrefabs;
    
    [SerializeField] private int girdWidth = 20;
    [SerializeField] private int girdHeight = 20;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 4f;

    [SerializeField] private float foodLifetime = 10f;
    [SerializeField] private float powerUpLifetime = 7f;

    private SnakeController snakeController;

    
    // Start is called before the first frame update
    void Start()
    {
        snakeController = FindObjectOfType<SnakeController>();
        StartCoroutine(SpawnFoodRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnFoodRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnRandomFood();
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnRandomPowerUp();
        }
    }

    private void SpawnRandomFood()
    {
        Vector2Int foodPosition = GetRandomPosition();

        GameObject foodPrefab = ChooseFoodType();

        if(foodPrefab != null)
        {
            GameObject food = Instantiate(foodPrefab, new Vector3(foodPosition.x, foodPosition.y, 0), Quaternion.identity);
            Destroy(food, foodLifetime);
        }
    }

    private void SpawnRandomPowerUp()
    {
        Vector2Int powerUpPosition = GetRandomPosition();

        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

        GameObject powerUp = Instantiate(powerUpPrefab, new Vector3(powerUpPosition.x, powerUpPosition.y, 0), Quaternion.identity);
        Destroy(powerUp, powerUpLifetime);

    }

    private Vector2Int GetRandomPosition()
    {
        Vector2Int randomPosition;

        do{
            int x = Random.Range(0, girdWidth);
            int y = Random.Range(0, girdHeight);
            randomPosition = new Vector2Int(x, y);
        }
        while (snakeController.IsOccupiedBySnake(randomPosition));

        return randomPosition;
    }

    private GameObject ChooseFoodType()
    {
        int size = snakeController.GetSnakeSize();
        bool canSpawnBurner = size > 1;
        
        if (!canSpawnBurner)
            return massGainerPrefab;

        return Random.value > 0.3 ? massGainerPrefab : massBurnerPrefab;
    }

    // public void SpawnFood()
    // {
    //     Vector2Int foodPosition;
    //     do{
    //         int x = Random.Range(0, girdWidth);
    //         int y = Random.Range(0, girdHeight);
    //         foodPosition = new Vector2Int(x, y);
    //     }
    //     while (IsOccupiedBySnake(foodPosition));

    //     Instantiate(foodPrefab, new Vector3(foodPosition.x, foodPosition.y, 0), Quaternion.identity);
    // }

    
}

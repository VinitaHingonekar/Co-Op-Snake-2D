using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject massGainerPrefab;
    public GameObject massBurnerPrefab;
    [SerializeField] private int girdWidth = 20;
    [SerializeField] private int girdHeight = 20;
    [SerializeField] private float minSpawnInterval = 3f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private float foodLifetime = 4f;

    private SnakeController snakeController;

    
    // Start is called before the first frame update
    void Start()
    {
        snakeController = FindObjectOfType<SnakeController>();
        StartCoroutine(SpawnFoodRoutine());
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

    private void SpawnRandomFood()
    {
        Vector2Int foodPosition;
        do{
            int x = Random.Range(0, girdWidth);
            int y = Random.Range(0, girdHeight);
            foodPosition = new Vector2Int(x, y);
        }
        while (IsOccupiedBySnake(foodPosition));

        GameObject foodPrefab = ChooseFoodType();

        if(foodPrefab != null)
        {
            GameObject food = Instantiate(foodPrefab, new Vector3(foodPosition.x, foodPosition.y, 0), Quaternion.identity);
            Destroy(food, foodLifetime);
        }

    }

    private GameObject ChooseFoodType()
    {
        int size = snakeController.GetSnakeSize();
        bool canSpawnBurner = size > 1;
        
        if (!canSpawnBurner)
            return massGainerPrefab;

        return Random.value > 0.5 ? massGainerPrefab : massBurnerPrefab;
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

    private bool IsOccupiedBySnake(Vector2Int position)
    {
        if (snakeController.GetSnakeHeadPosition() == position)
            return true;
        
        foreach(Transform bodyPart in snakeController.GetSnakeBodyParts())
        {
            if(Vector2Int.RoundToInt(bodyPart.position) == position)
                return true;
        }

        return false;
    }
}

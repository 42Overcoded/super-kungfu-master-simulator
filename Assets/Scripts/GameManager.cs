using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentRound = 0;
    private int difficulty_setting = 0; //0 = easy, 1 = medium, 2 = hard
    public float current_difficulty = 1; //the difficulty will depend on the current round and the difficulty setting. It should be used to determine the number of enemies and their strength
    private float roundTime = 60f; //Duration of each round in seconds
    private float gameTime = 0f; //Variable to keep track of the time if the current round
    private float transitionTimer = 0f; //Timer to keep track of the time between rounds
    public int score = 0;
    private int gameState = 0; //0 = round starting, 1 = round in progress, 2 = round ended, 3 = game over
    private GameObject player;
    public int numberOfEnemies = 0; //placeholder, number of enemies remaining in the round, this should be updated to function properly with the enemy spawner

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab; //Reference to the enemy prefab
    [SerializeField] private float spawnRadius = 10f; //Radius of the spawn circle around the player
    [SerializeField] private int baseEnemyCount = 3; //Base number of enemies per round
    [SerializeField] private float enemyCountMultiplier = 1.5f; //How much the enemy count increases per round

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player"); //Find the player object in the scene
        if (player == null) {
            Debug.LogError("Player object not found in the scene!");
        }

        current_difficulty += difficulty_setting * 2f; //Initialize the difficulty based on the difficulty setting
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            Debug.LogError("Player object not found in the scene!");
            return; //Exit if player is not found
        }
        //Updates the current round time
        gameTime += Time.deltaTime;
        roundUpdate();
    }

    private void roundUpdate()
    {
        switch (gameState) { //simple state machine to handle game states
            case 0: //Round start
                transitionTimer += Time.deltaTime;

                if (transitionTimer >= 3f) { //Wait for 3 seconds before starting the round
                    startRound();
                    gameState = 1; //Change to round in progress
                    Debug.Log("Round started! Current round: " + currentRound);
                }
                break;

            case 1: //Round in progress
                    //a round either ends when the time runs out or when all enemies are defeated
                    //bonus points will be awarded for defeating all enemies before the time runs out
                if (gameTime >= roundTime || numberOfEnemies <= 0) {
                    //If the round time is over or there are no enemies left, end the round
                    if (numberOfEnemies <= 0) {
                        AddScore((int)(roundTime)); //Bonus points for defeating all enemies
                        Debug.Log("All enemies defeated! Bonus points awarded.");
                    } else {
                        Debug.Log("Time's up! No bonus points awarded.");
                    }
                    gameState = 2; //Change to round ended
                    transitionTimer = 5f; //Set transition time before next round starts
                    Debug.Log("Round ended! Score: " + score);
                }
                break;

            case 2: //Round ended
                transitionTimer += Time.deltaTime;
                if (transitionTimer >= 3f) {
                    startRound(); //Start a new round
                    gameState = 0; //Change back to round starting
                }
                break;
            case 3: //Game over
                Debug.Log("Game Over! Final Score: " + score);
                break;
        }
    }

    //Starts a new round
    private void startRound()
    {
        currentRound++;
        gameTime = 0f;
        transitionTimer = 0f;
        updateDifficulty();

        Debug.Log("Starting round: " + currentRound);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab not assigned!");
            return;
        }

        // Calculate number of enemies based on round and difficulty
        int enemyCount = Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(enemyCountMultiplier, currentRound - 1) * current_difficulty);
        numberOfEnemies = enemyCount;

        // Spawn enemies in a circle around the player
        for (int i = 0; i < enemyCount; i++)
        {
            // Calculate angle for this enemy
            float angle = i * (360f / enemyCount);
            float radians = angle * Mathf.Deg2Rad;

            // Calculate position
            float x = player.transform.position.x + spawnRadius * Mathf.Cos(radians);
            float z = player.transform.position.z + spawnRadius * Mathf.Sin(radians);
            Vector3 spawnPosition = new Vector3(x, player.transform.position.y, z);

            // Spawn the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Make enemy face the player
            enemy.transform.LookAt(player.transform);
        }

        Debug.Log($"Spawned {enemyCount} enemies");
    }

    private void updateDifficulty()
    {
        current_difficulty += 1 + difficulty_setting * 0.5f; //Example formula to calculate difficulty based on round and difficulty setting
    }

    //Used whenever score needs to be updated
    public void AddScore(int points)
    {
        score += points;
    }

    public void SetDifficulty(int difficulty)
    {
        difficulty_setting = difficulty;
        Debug.Log("Difficulty set to: " + difficulty_setting);
    }

    public void EndGame()
    {
        gameState = 3; //Set game state to game over
        Debug.Log("Game Over! Final Score: " + score);
    }
}

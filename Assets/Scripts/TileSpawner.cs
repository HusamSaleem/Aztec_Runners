using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    private PlayerManager playerManager;
    // Tiles
    public GameObject normalTile;
    public GameObject player;
    public GameObject[] obstacleTiles;
    public GameObject[] unitTiles;
    public GameObject referenceObject;
    private Queue<GameObject> currentTiles; // Keep all tiles in a queue, remove the first ones in for optimization

    // Constants
    private float maxCapacity = 20;
    private float distanceToSpawnTile = 10.0f;
    private float distanceBetweenTiles = 5.2f;
    private float changeDirectionChance = 0.3f;
    [System.NonSerialized]
    public float spawnObstacleChance = 0.375f;
    [System.NonSerialized]
    public float spawnCreditChance = 0.45f;
    [System.NonSerialized]
    public int gracePeriod = 7; // 7 tiles spawn before any obstacle or unit tiles
    [System.NonSerialized]
    public bool gameStarted = true;

    // Tile spawning info
    private bool isPreviousTileObstacle = false;
    private Vector3 previousTilePosition;
    private float previousRotationY = 0;
    private bool changedDir = false;
    private Vector3 direction, mainDirection = Vector3.forward, otherDirection = Vector3.right;

    private void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    void Start()
    {
        currentTiles = new Queue<GameObject>();
        previousTilePosition = referenceObject.transform.position;
    } 

    void Update()
    {
        if (gameStarted)
        {
            if (playerManager.data.directionUnlocked)
            {
                SpawnTilesV2();
            } else
            {
                SpawnTiles();
            }

            if (currentTiles.Count >= maxCapacity)
            {
                Destroy(currentTiles.Dequeue());
            }
        }
    }

    // Tiles can change direction based on what player wants
    private void SpawnTilesV2()
    {
        if (playerManager.changedDirTime <= 0)
        {
            changedDir = false;
        } else
        {
            playerManager.changedDirTime -= Time.deltaTime;
        }

        float curRotation = Mathf.Abs(previousRotationY - player.transform.rotation.eulerAngles.y);
        if (curRotation == 0 || changedDir)
        {
            direction = mainDirection;
        }
        if (Input.GetKeyDown(KeyCode.Q) && !changedDir)
        {
            previousRotationY = player.transform.rotation.eulerAngles.y;
            direction = player.transform.right * -1;
            mainDirection = direction;
            previousTilePosition = playerManager.currentTile.transform.position;
            changedDir = true;
            playerManager.changedDirTime = playerManager.data.changeDirCooldown;
        }
        if (Input.GetKeyDown(KeyCode.E) && !changedDir)
        {
            previousRotationY = player.transform.rotation.eulerAngles.y;
            direction = player.transform.right;
            mainDirection = direction;
            previousTilePosition = playerManager.currentTile.transform.position;
            changedDir = true;
            playerManager.changedDirTime = playerManager.data.changeDirCooldown;
        }

        float dist = Vector3.Distance(previousTilePosition, player.transform.position);
        if (dist <= distanceToSpawnTile)
        {
            // Obstacles or coin tiles?
            if (Random.value < spawnCreditChance && currentTiles.Count >= gracePeriod)
            {
                direction = mainDirection;
                SpawnTile(unitTiles[Random.Range(0, unitTiles.Length)]);
                return;
            }
            else if (Random.value < spawnObstacleChance && currentTiles.Count >= gracePeriod)
            {
                direction = mainDirection;
                SpawnTile(obstacleTiles[Random.Range(0, obstacleTiles.Length)]);
                isPreviousTileObstacle = true;
                return;
            }

            SpawnTile(normalTile);
        }
    }

    // Tiles & direction spawn randomly
    private void SpawnTiles()
    {
        float dist = Vector3.Distance(previousTilePosition, player.transform.position);

        if (dist <= distanceToSpawnTile)
        {
            if (currentTiles.Count < gracePeriod)
            {
                direction = mainDirection;
                SpawnTile(normalTile);
                return;
            }

            // Change directions?
            if (Random.value < changeDirectionChance && !isPreviousTileObstacle)
            {
                Vector3 temp = direction;
                direction = otherDirection;
                mainDirection = direction;
                otherDirection = temp;

                SpawnTile(normalTile);
                return;
            }

            // Obstacles or coin tiles?
            if (Random.value < spawnCreditChance)
            {
                direction = mainDirection;
                SpawnTile(unitTiles[Random.Range(0, unitTiles.Length)]);
                return;
            } else if (Random.value < spawnObstacleChance)
            {
                direction = mainDirection;
                SpawnTile(obstacleTiles[Random.Range(0, obstacleTiles.Length)]);
                isPreviousTileObstacle = true;
                return;
            }

            direction = mainDirection;
            SpawnTile(normalTile);
        }
    }

    private void SpawnTile(GameObject tile)
    {
        previousTilePosition.y = 0;
        Vector3 spawnPos = previousTilePosition + distanceBetweenTiles * direction;
        GameObject newTile = Instantiate(tile, spawnPos, Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0));
        currentTiles.Enqueue(newTile);
        previousTilePosition = spawnPos;
        isPreviousTileObstacle = false;
    }

    public void StopGame()
    {
        gameStarted = false;
        ResetDirections();
        while (currentTiles.Count > 0)
        {
            Destroy(currentTiles.Dequeue());
        }
    }

    public void StartGame()
    {
        previousTilePosition = referenceObject.transform.position;
        gameStarted = true;
    }

    private void ResetDirections()
    {
        mainDirection = new Vector3(0, 0, 1);
        otherDirection = new Vector3(1, 0, 0);
        direction = Vector3.zero;
        previousRotationY = 0;
        changedDir = false;
    }
}

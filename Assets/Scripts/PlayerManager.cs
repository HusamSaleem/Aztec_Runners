using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private TileSpawner tileSpawner;
    private MenuManager menuManager;
    [SerializeField]
    private Canvas deathScreen;
    [SerializeField]
    private Canvas playerUi;
    private GuiManager guiManager;
    public PlayerData data;
    private Animator animator;
    private ShopManager shopManager;
    private AudioManager audioManager;
    private Quest quests;

    // Movement
    private int deathHeight = -5;
    private int gravity = -10;
    [System.NonSerialized]
    public bool dead = false;
    private bool turnLeft, turnRight;
    [System.NonSerialized]
    public Vector3 playerRotation = Vector3.forward;
    private CharacterController characterController;
    private Vector3 moveVelocity = Vector3.zero;
    [System.NonSerialized]
    public float changedDirTime = 0.0f;
    [System.NonSerialized]
    public float timeSurvived = 0.0f;
    [System.NonSerialized]
    public GameObject currentTile; // Important to know what tile the player is stepping on

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        tileSpawner = GameObject.Find("Camera").GetComponent<TileSpawner>();
        guiManager = GameObject.Find("UiManager").GetComponent<GuiManager>();
        shopManager = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
        menuManager = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MenuManager>();
        audioManager = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<AudioManager>();
        quests = GameObject.FindGameObjectWithTag("Shop").GetComponent<Quest>();
        ApplyDifficulty(menuManager.selectedDifficulty, menuManager.directionUnlockedMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= deathHeight && !dead)
        {
            PlayerDied();
        } else
        {
            timeSurvived += Time.deltaTime;
            guiManager.UpdatePlayerTexts();
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        turnLeft = Input.GetKeyDown(KeyCode.A);
        turnRight = Input.GetKeyDown(KeyCode.D);
        // Rotate character left or right
        if (turnLeft)
        {
            playerRotation = transform.right * -1;
            transform.Rotate(new Vector3(0f, -90f, 0f));
        }
        else if (turnRight)
        {
            playerRotation = transform.right;
            transform.Rotate(new Vector3(0f, 90f, 0f));
        }

        if (characterController.isGrounded)
        {
            animator.SetBool("Jump", false);
            moveVelocity = transform.forward * data.speed;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveVelocity.y = data.jumpSpeed;
                animator.SetBool("Jump", true);
            }
        }
        moveVelocity.y += gravity * Time.deltaTime;
        characterController.Move(moveVelocity * Time.deltaTime);
    }

    private void ApplyDifficulty(Difficulty difficulty, bool directionUnlocked)
    {
        data.directionUnlocked = directionUnlocked;
        if (!difficulty.gracePeriod)
        {
            tileSpawner.gracePeriod = 0;
        }
        data.perCreditCollected = data.perCreditCollected + (data.perCreditCollected * difficulty.creditMultiplier);
        tileSpawner.spawnObstacleChance = tileSpawner.spawnObstacleChance + (tileSpawner.spawnObstacleChance * difficulty.obstacleSpawnMultiplier);
        tileSpawner.spawnCreditChance = tileSpawner.spawnCreditChance + (tileSpawner.spawnCreditChance * difficulty.creditSpawnMultiplier);
        data.creditsNeeded = difficulty.creditsNeeded;
        shopManager.ApplyUpgradeCosts(difficulty.upgradeCostMultiplier);
    }

    private void PlayerDied()
    {
        audioManager.LowerVolume();
        playerUi.gameObject.SetActive(false);
        data.credits += data.creditsCollected;
        dead = true;
        tileSpawner.StopGame();
        changedDirTime = 0.0f;
        deathScreen.gameObject.SetActive(true);
        quests.CheckForCompletions();
        guiManager.UpdateDeathScreenTexts();
    }

    public void RestartGame()
    {
        data.creditsCollected = 0;
        tileSpawner.StartGame();
        audioManager.IncreaseVolume();
        deathScreen.gameObject.SetActive(false);
        ResetCharacter();
        timeSurvived = 0.0f;
        playerUi.gameObject.SetActive(true);
    }

    private void ResetCharacter()
    {
        characterController.enabled = false;
        transform.position = Vector3.zero;
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        characterController.enabled = true;
        dead = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            PlayerDied();
        }
        if (other.tag == "Credit")
        {
            Destroy(other.gameObject);
            data.creditsCollected = (float)Math.Round(data.creditsCollected + data.perCreditCollected, 3);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Tile")
        {
            currentTile = hit.gameObject;
        }
    }
}

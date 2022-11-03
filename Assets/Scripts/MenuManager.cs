using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI selectedDifficultyTxt;
    public TextMeshProUGUI selectedModeTxt;
    public GameObject mainPanel;
    public GameObject modePanel;

    private int difficultyIndex = -1;
    private Difficulty[] difficultyMap;
    [System.NonSerialized]
    public Difficulty selectedDifficulty;
    [System.NonSerialized]
    public bool directionUnlockedMode = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        difficultyMap = new Difficulty[4];
        CreateDifficulties();
        UpdateUI();
    }

    public void GoToModePanel()
    {
        modePanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void PlayGame()
    {
        if (difficultyIndex == -1) return;
        SceneManager.LoadScene(1);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void GoToMainPanel()
    {
        modePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void SelectDifficulty(int difficulty)
    {
        difficultyIndex = difficulty;
        selectedDifficulty = difficultyMap[difficultyIndex];
        UpdateUI();
    }

    public void SetMode(bool directionUnlocked)
    {
        this.directionUnlockedMode = directionUnlocked;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (selectedDifficultyTxt == null || selectedModeTxt == null)
        {
            return;
        }
        if (difficultyIndex == -1)
        {
            selectedDifficultyTxt.text = "Selected Difficulty: None";
        } else
        {
            selectedDifficultyTxt.text = "Selected Difficulty: " + difficultyMap[difficultyIndex].name;
        }
        
        if (!directionUnlockedMode)
        {
            selectedModeTxt.text = "Selected Mode: 1";
        } else
        {
            selectedModeTxt.text = "Selected Mode: 2";
        }
    }

    private void CreateDifficulties()
    {
        Difficulty easy = new Difficulty();
        easy.name = "Easy";
        easy.difficultyIndex = 0;
        easy.creditsNeeded = 60;
        easy.creditSpawnMultiplier = 0.1f;
        easy.obstacleSpawnMultiplier = 0f;
        easy.creditMultiplier = 0.1f;
        easy.upgradeCostMultiplier = -0.05f;
        easy.gracePeriod = true;

        Difficulty normal = new Difficulty();
        normal.name = "Normal";
        normal.difficultyIndex = 1;
        normal.creditsNeeded = 120;
        normal.creditMultiplier = 0f;
        normal.obstacleSpawnMultiplier = 0f;
        normal.upgradeCostMultiplier = 0f;
        normal.creditSpawnMultiplier = 0f;
        normal.gracePeriod = true;

        Difficulty hard = new Difficulty();
        hard.name = "Hard";
        hard.difficultyIndex = 2;
        hard.creditsNeeded = 160;
        hard.creditMultiplier = -0.05f;
        hard.obstacleSpawnMultiplier = 0.12f;
        hard.upgradeCostMultiplier = 0.15f;
        hard.creditSpawnMultiplier = -0.05f;
        hard.gracePeriod = true;

        Difficulty insane = new Difficulty();
        insane.name = "Insane";
        insane.difficultyIndex = 3;
        insane.creditsNeeded = 250;
        insane.creditMultiplier = -0.15f;
        insane.obstacleSpawnMultiplier = 0.25f;
        insane.upgradeCostMultiplier = 0.33f;
        insane.creditSpawnMultiplier = -0.15f;
        insane.gracePeriod = false;

        difficultyMap[0] = easy;
        difficultyMap[1] = normal;
        difficultyMap[2] = hard;
        difficultyMap[3] = insane;
    }
}

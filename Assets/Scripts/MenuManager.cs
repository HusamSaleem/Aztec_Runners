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
    public Difficulty[] difficulties;
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
        selectedDifficulty = difficulties[difficultyIndex];
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
            selectedDifficultyTxt.text = "Selected Difficulty: " + difficulties[difficultyIndex].name;
        }
        
        if (!directionUnlockedMode)
        {
            selectedModeTxt.text = "Selected Mode: 1";
        } else
        {
            selectedModeTxt.text = "Selected Mode: 2";
        }
    }
}

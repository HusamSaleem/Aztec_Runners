using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public TextMeshProUGUI playerCredits;
    private TileSpawner tileManager;
    private Quest quests;

    // Buttons
    public Button upgradeSpeedBtn;
    public Button upgradeCreditsMultiplierBtn;
    public Button upgradeCreditSpawningBtn;
    public Button upgradeLessObstaclesBtn;
    public Button upgradeQuestRewardBtn;

    // Upgrades
    public Upgrade speedUpgrade;
    public Upgrade creditMultiplierUpgrade;
    public Upgrade increasedCreditSpawn;
    public Upgrade lessObstacles;
    public Upgrade questReward;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        tileManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TileSpawner>();
        quests = GameObject.FindGameObjectWithTag("Shop").GetComponent<Quest>();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (speedUpgrade.maxed)
        {
            upgradeSpeedBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Speed";
        } else
        {
            upgradeSpeedBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + speedUpgrade.statModifier[speedUpgrade.level] + " speed (" + speedUpgrade.costs[speedUpgrade.level] + "Cr)";
        }

        if (creditMultiplierUpgrade.maxed)
        {
            upgradeCreditsMultiplierBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Credit Multiplier";
        } else
        {
            upgradeCreditsMultiplierBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + creditMultiplierUpgrade.statModifier[creditMultiplierUpgrade.level] + "% credits multiplier (" + creditMultiplierUpgrade.costs[creditMultiplierUpgrade.level] + "Cr)";
        }

        if (increasedCreditSpawn.maxed)
        {
            upgradeCreditSpawningBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Increased Credit Spawn";
        } else
        {
            upgradeCreditSpawningBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + increasedCreditSpawn.statModifier[increasedCreditSpawn.level] + "% credits spawned (" + increasedCreditSpawn.costs[increasedCreditSpawn.level] + "Cr)";
        }

        if (lessObstacles.maxed)
        {
            upgradeLessObstaclesBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Increased Credit Spawn";
        }
        else
        {
            upgradeLessObstaclesBtn.GetComponentInChildren<TextMeshProUGUI>().text = lessObstacles.statModifier[lessObstacles.level] + "% obstacles (" + lessObstacles.costs[lessObstacles.level] + "Cr)";
        }

        if (questReward.maxed)
        {
            upgradeQuestRewardBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Quest Reward";
        } else
        {
            upgradeQuestRewardBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + questReward.statModifier[questReward.level] + "% quest reward (" + questReward.costs[questReward.level] + "Cr)";
        }
        playerCredits.text = playerManager.data.credits + " credits";
    }

    public void ApplyUpgradeCosts(float costMultiplier)
    {
        speedUpgrade.ModifyCost(costMultiplier);
        creditMultiplierUpgrade.ModifyCost(costMultiplier);
        lessObstacles.ModifyCost(costMultiplier);
        increasedCreditSpawn.ModifyCost(costMultiplier);
        UpdateUI();
    }

    public void BuySpeedUpgrade()
    {
        if (!speedUpgrade.CanPurchase(playerManager.data.credits)) return;
        playerManager.data.credits -= speedUpgrade.costs[speedUpgrade.level];
        ApplySpeedUpgrade();
        speedUpgrade.level++;
        UpdateUI();
    }

    private void ApplySpeedUpgrade()
    {
        playerManager.data.speed += speedUpgrade.statModifier[speedUpgrade.level];
    }

    public void BuyCreditMultiplierUpgrade()
    {
        if (!creditMultiplierUpgrade.CanPurchase(playerManager.data.credits)) return;
        playerManager.data.credits -= creditMultiplierUpgrade.costs[creditMultiplierUpgrade.level];
        ApplyCreditMultiplier();
        creditMultiplierUpgrade.level++;
        UpdateUI();
    }

    private void ApplyCreditMultiplier()
    {
        playerManager.data.perCreditCollected += (playerManager.data.perCreditCollected * (creditMultiplierUpgrade.statModifier[creditMultiplierUpgrade.level] / 100));
    }

    public void BuyIncreasedCreditSpawn()
    {
        if (!increasedCreditSpawn.CanPurchase(playerManager.data.credits)) return;
        playerManager.data.credits -= increasedCreditSpawn.costs[increasedCreditSpawn.level];
        IncreaseCreditSpawnRate();
        increasedCreditSpawn.level++;
        UpdateUI();
    }

    private void IncreaseCreditSpawnRate()
    {
        tileManager.spawnCreditChance = tileManager.spawnCreditChance + (tileManager.spawnCreditChance * (increasedCreditSpawn.statModifier[increasedCreditSpawn.level] / 100));
    }

    public void BuyLessObstaclesUpgrade()
    {
        if (!lessObstacles.CanPurchase(playerManager.data.credits)) return;
        playerManager.data.credits -= lessObstacles.costs[lessObstacles.level];
        LessObstacles();
        lessObstacles.level++;
        UpdateUI();
    }

    private void LessObstacles()
    {
        tileManager.spawnObstacleChance = tileManager.spawnObstacleChance + (tileManager.spawnObstacleChance * (lessObstacles.statModifier[lessObstacles.level] / 100));
    }

    public void BuyQuestRewardUpgrade()
    {
        if (!questReward.CanPurchase(playerManager.data.credits)) return;
        playerManager.data.credits -= questReward.costs[questReward.level];
        IncreaseQuestReward();
        questReward.level++;
        UpdateUI();
    }

    private void IncreaseQuestReward()
    {
        quests.questReward = quests.questReward + (quests.questReward * (questReward.statModifier[questReward.level] / 100));
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public TextMeshProUGUI playerCredits;

    // Buttons
    public Button upgradeSpeedBtn;
    public Button upgradeCreditsMultiplierBtn;

    // Upgrades
    public Upgrade speedUpgrade;
    public Upgrade creditMultiplierUpgrade;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
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
            upgradeSpeedBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + speedUpgrade.statModifier[speedUpgrade.level] + " speed (" + speedUpgrade.costs[speedUpgrade.level] + " credits)";
        }

        if (creditMultiplierUpgrade.maxed)
        {
            upgradeCreditsMultiplierBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Credit Multiplier";
        } else
        {
            upgradeCreditsMultiplierBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + creditMultiplierUpgrade.statModifier[creditMultiplierUpgrade.level] + "% credits multiplier (" + creditMultiplierUpgrade.costs[creditMultiplierUpgrade.level] + " credits)";
        }
        playerCredits.text = playerManager.data.credits + " credits";
    }

    public void ApplyUpgradeCosts(float costMultiplier)
    {
        speedUpgrade.ModifyCost(costMultiplier);
        creditMultiplierUpgrade.ModifyCost(costMultiplier);
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
}
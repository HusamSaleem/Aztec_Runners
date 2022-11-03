using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public TextMeshProUGUI playerCredits;

    // Speed upgrade
    private float[] speedUpgrades = { 0.25f, 0.5f, 0.75f, 1.0f, 1.5f };
    private float[] speedUpgradeCost = { 0.5f, 0.88f, 1.7f, 2.9f, 4.5f };
    private int curSpeedUpgrade = -1;
    public Button upgradeSpeedBtn;

    // Credits collected upgrade
    private float[] creditsMultiplier = { 10, 20, 33f, 40f, 65f }; // Upgrade by %
    private float[] creditsMultiplierUpgradeCost = { 1.5f, 5f, 7f, 10.2f, 17.5f };
    private int curCreditsMultiplierUpgrade = -1;
    public Button upgradeCreditsMultiplierBtn;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (curSpeedUpgrade == speedUpgrades.Length)
        {
            upgradeSpeedBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Speed";
        } else
        {
            int temp = curSpeedUpgrade == -1 ? 1 : 0;
            upgradeSpeedBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + speedUpgrades[curSpeedUpgrade + temp] + " speed (" + speedUpgradeCost[curSpeedUpgrade + temp] + " credits)";
        }

        if (curCreditsMultiplierUpgrade == creditsMultiplier.Length)
        {
            upgradeCreditsMultiplierBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Maxed Credit Multiplier";
        }
        else
        {
            int temp = curCreditsMultiplierUpgrade == -1 ? 1 : 0;
            upgradeCreditsMultiplierBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + creditsMultiplier[curCreditsMultiplierUpgrade + temp] + "% credits multiplier (" + creditsMultiplierUpgradeCost[curCreditsMultiplierUpgrade + temp] + " credits)";
        }
        playerCredits.text = playerManager.credits + " credits";
    }

    public void ApplyUpgradeCosts(float costMultiplier)
    {
        for (int i = 0; i < speedUpgradeCost.Length; i++)
        {
            speedUpgradeCost[i] = speedUpgradeCost[i] + (speedUpgradeCost[i] * costMultiplier);
        }

        for (int i = 0; i < creditsMultiplierUpgradeCost.Length; i++)
        {
            creditsMultiplierUpgradeCost[i] = creditsMultiplierUpgradeCost[i] + (creditsMultiplierUpgradeCost[i] * costMultiplier);
        }
        UpdateUI();
    }

    public void BuySpeedUpgrade()
    {
        if (curSpeedUpgrade == speedUpgrades.Length) return;
        if (curSpeedUpgrade == -1) curSpeedUpgrade = 0;
        if (playerManager.credits < speedUpgradeCost[curSpeedUpgrade]) return;
        playerManager.credits -= speedUpgradeCost[curSpeedUpgrade];
        ApplySpeedUpgrade();
        curSpeedUpgrade++;
        UpdateUI();
    }

    private void ApplySpeedUpgrade()
    {
        if (curSpeedUpgrade == -1) return;
        playerManager.speed += speedUpgrades[curSpeedUpgrade];
    }

    public void BuyCreditMultiplierUpgrade()
    {
        if (curCreditsMultiplierUpgrade == creditsMultiplier.Length) return;
        if (curCreditsMultiplierUpgrade == -1) curCreditsMultiplierUpgrade = 0;
        if (playerManager.credits < creditsMultiplierUpgradeCost[curCreditsMultiplierUpgrade]) return;
        playerManager.credits -= creditsMultiplierUpgradeCost[curCreditsMultiplierUpgrade];
        ApplyCreditMultiplier();
        curCreditsMultiplierUpgrade++;
        UpdateUI();
    }

    private void ApplyCreditMultiplier()
    {
        if (curCreditsMultiplierUpgrade == -1) return;
        playerManager.perCreditCollected += (playerManager.perCreditCollected * (creditsMultiplier[curCreditsMultiplierUpgrade] / 100));
    }
}
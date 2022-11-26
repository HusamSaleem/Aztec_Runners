using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    // Death screen UI
    public Canvas deathScreen;
    public TextMeshProUGUI creditsEarned;
    public TextMeshProUGUI timeSurvived;
    public TextMeshProUGUI totalCredits;
    public TextMeshProUGUI log;

    // Player UI
    private PlayerManager playerManager;
    public TextMeshProUGUI playerCreditsEarned;
    public TextMeshProUGUI playerTimeSurvived;
    public TextMeshProUGUI changeDirCooldown;
    private Quest quests;

    // For spawn tiles v2
    public Image cooldownTimer;
    public TextMeshProUGUI cooldownTxt;
    public TextMeshProUGUI directionInfoTxt;

    // Shop UI
    public Canvas shopScreen;

    private void Awake()
    {
        cooldownTimer.gameObject.SetActive(false);
        cooldownTxt.gameObject.SetActive(false);
        directionInfoTxt.gameObject.SetActive(false);
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        quests = GameObject.FindGameObjectWithTag("Shop").GetComponent<Quest>();
    }

    public void Retry()
    {
        log.gameObject.SetActive(false);
        playerManager.RestartGame();
    }

    public void UpdateDeathScreenTexts()
    {
        creditsEarned.text = "Credits Earned: " + playerManager.data.creditsCollected;
        timeSurvived.text = "Time Survived: " + Math.Round(playerManager.timeSurvived, 2) + "s";
        totalCredits.text = "Total Credits: " + playerManager.data.credits;
    }

    public void UpdatePlayerTexts()
    {
        playerCreditsEarned.text = playerManager.data.creditsCollected.ToString();
        playerTimeSurvived.text = Math.Round(playerManager.timeSurvived, 2).ToString();

        if (playerManager.data.directionUnlocked)
        {
            ShowExtraUI();
            changeDirCooldown.text = Math.Round(playerManager.changedDirTime, 2).ToString();
        }
    }

    public void OpenShop()
    {
        shopScreen.gameObject.SetActive(true);
        deathScreen.gameObject.SetActive(false);
    }

    public void CloseShop()
    {
        shopScreen.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(true);
        quests.CheckForCompletions();
    }

    private void ShowExtraUI()
    {
        cooldownTimer.gameObject.SetActive(true);
        cooldownTxt.gameObject.SetActive(true);
        directionInfoTxt.gameObject.SetActive(true);
    }

    public void Graduate()
    {
        if (playerManager.data.graduated)
        {
            log.gameObject.SetActive(true);
            log.text = "You have already graduated dummy";
            return;
        }

        if (playerManager.data.credits >= playerManager.data.creditsNeeded)
        {
            playerManager.data.credits -= playerManager.data.creditsNeeded;
            playerManager.data.graduated = true;
            totalCredits.text = "Total Credits: " + playerManager.data.credits;
            log.gameObject.SetActive(true);
            log.text = "Congratulations, you have graduated!! You can continue to play the game";
            quests.CheckForCompletions();
        } else
        {
            int creditsNeeded = (int)Math.Ceiling(playerManager.data.creditsNeeded - playerManager.data.credits);
            log.gameObject.SetActive(true);
            log.text = "You need " + creditsNeeded + " credits to graduate. Keep grinding";
        }
    }
}

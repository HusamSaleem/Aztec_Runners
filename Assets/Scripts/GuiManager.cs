using System;
using System.Collections;
using System.Collections.Generic;
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

    // Player UI
    private PlayerManager playerManager;
    public TextMeshProUGUI playerCreditsEarned;
    public TextMeshProUGUI playerTimeSurvived;
    public TextMeshProUGUI changeDirCooldown;

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
    }

    public void Retry()
    {
        playerManager.RestartGame();
    }

    public void UpdateDeathScreenTexts()
    {
        creditsEarned.text = "Credits Earned: " + playerManager.creditsCollected;
        timeSurvived.text = "Time Survived: " + Math.Round(playerManager.timeSurvived, 2) + "s";
        totalCredits.text = "Total Credits: " + playerManager.credits;
    }

    public void UpdatePlayerTexts()
    {
        playerCreditsEarned.text = playerManager.creditsCollected.ToString();
        playerTimeSurvived.text = Math.Round(playerManager.timeSurvived, 2).ToString();

        if (playerManager.directionUnlocked)
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
    }

    private void ShowExtraUI()
    {
        cooldownTimer.gameObject.SetActive(true);
        cooldownTxt.gameObject.SetActive(true);
        directionInfoTxt.gameObject.SetActive(true);
    }
}

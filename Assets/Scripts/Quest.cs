using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    PlayerManager playerManager;
    ShopManager shopManager;
    public float questReward = 1.05f;
    public Objective timeObjective;
    public Objective creditObjective;
    public Objective speedObjective;
    public Objective creditMultiplierObjective;
    public Objective playerGraduatedObjective;

    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI creditTxt;
    public TextMeshProUGUI speedTxt;
    public TextMeshProUGUI creditMultiplierTxt;
    public TextMeshProUGUI graduatedTxt;


    private void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        shopManager = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
    }

    public void UpdateUI()
    {
        if (!timeObjective.completed)
        {
            timeTxt.text = "Survive at least " + Math.Round(timeObjective.timeNeeded, 2) + " seconds (+" + Math.Round(timeObjective.reward, 2) + "Cr)";
        } else
        {
            timeTxt.gameObject.SetActive(false);
        }

        if (!creditObjective.completed)
        {
            creditTxt.text = "Earn " + Math.Round(creditObjective.creditsNeeded, 2) + " credits in one run(+" + Math.Round(creditObjective.reward, 2) + "Cr)";
        }
        else
        {
            creditTxt.gameObject.SetActive(false);
        }

        if (!speedObjective.completed)
        {
            speedTxt.text = "Max out the speed upgrade " + "(+" + speedObjective.reward + "Cr)";
        }
        else
        {
            speedTxt.gameObject.SetActive(false);
        }

        if (!creditMultiplierObjective.completed)
        {
            creditMultiplierTxt.text = "Max out the credit multiplier upgrade " + "(+" + creditMultiplierObjective.reward + "Cr)";
        }
        else
        {
            creditMultiplierTxt.gameObject.SetActive(false);
        }

        if (!playerGraduatedObjective.completed)
        {
            graduatedTxt.text = "Graduate" + "(+" + playerGraduatedObjective.reward + "Cr)";
        }
        else
        {
            graduatedTxt.gameObject.SetActive(false);
        }
    }

    public void CheckForCompletions()
    {
        CheckTimeRun();
        CheckCreditsRun();
        CheckSpeedUpgrade();
        CheckMultiplierUpgrade();
        CheckGraduated();
        UpdateUI();
    }

    private void CheckGraduated()
    {
        if (!playerGraduatedObjective.completed && playerManager.data.graduated)
        {
            playerManager.data.credits += playerGraduatedObjective.reward;
            playerGraduatedObjective.completed = true;
        }
    }

    private void CheckMultiplierUpgrade()
    {
        if (!creditMultiplierObjective.completed && shopManager.creditMultiplierUpgrade.maxed)
        {
            playerManager.data.credits += creditMultiplierObjective.reward;
            creditMultiplierObjective.completed = true;
        }
    }

    private void CheckSpeedUpgrade()
    {
        if (!speedObjective.completed && shopManager.speedUpgrade.maxed)
        {
            playerManager.data.credits += speedObjective.reward;
            speedObjective.completed = true;
        }
    }

    private void CheckTimeRun()
    {
        if (!timeObjective.completed && timeObjective.timeNeeded <= playerManager.timeSurvived)
        {
            playerManager.data.credits += timeObjective.reward;
            timeObjective.reward *= questReward;
            timeObjective.timeNeeded *= 1.25f;
        }
    }

    private void CheckCreditsRun()
    {
        if (!creditObjective.completed && creditObjective.creditsNeeded <= playerManager.data.creditsCollected)
        {
            playerManager.data.credits += creditObjective.reward;
            creditObjective.reward *= questReward;
            creditObjective.creditsNeeded *= 1.25f;
        }
    }
}

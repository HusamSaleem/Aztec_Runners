using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Objective : ScriptableObject
{
    public float timeNeeded;
    public float creditsNeeded;
    public bool maxedOutSpeed;
    public bool maxedOutCreditMultiplier;
    public bool graduated;
    public float reward;
    public bool completed;
}

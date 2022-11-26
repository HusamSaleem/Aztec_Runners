using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public float speed;
    public float jumpSpeed;
    public float perCreditCollected;
    public bool directionUnlocked;
    public float changeDirCooldown;
    public float creditsCollected;
    public float creditsNeeded;
    public float credits;
    public bool graduated = false;
}
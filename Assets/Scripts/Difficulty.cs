
using UnityEngine;

[CreateAssetMenu]
public class Difficulty : ScriptableObject
{
    public new string name;
    public int creditsNeeded;
    public float creditSpawnMultiplier;
    public float obstacleSpawnMultiplier;
    public float creditMultiplier;
    public float upgradeCostMultiplier;
    public bool gracePeriod;
}

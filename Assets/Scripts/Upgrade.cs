using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public new string name;
    public int level = 0;
    public float[] costs;
    public float[] statModifier;
    public bool modifierInPercent;
    public bool maxed { get { return level == costs.Length; } }

    public void ModifyCost(float costMultiplier)
    {
        for (int i = 0; i < costs.Length; i++)
        {
            costs[i] = costs[i] + (costs[i] * costMultiplier);
        }
    }

    public bool CanPurchase(float playerCredits)
    {
        if (maxed) return false;
        if (playerCredits < costs[level]) return false;
        return true;
    }
}

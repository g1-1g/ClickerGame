using System;
using System.Collections;


[Serializable]
public class UpgradeSpecData
{
    public EUpgradeType Type;
    public int MaxLevel;
    public double BaseCost;
    public double BaseHeartGet;
    public double CostMultiplier;
    public double HeartGetMultiplier;
    public string Name;
    public string Description;
}
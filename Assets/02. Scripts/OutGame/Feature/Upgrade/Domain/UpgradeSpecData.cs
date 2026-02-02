using System;
using System.Collections;


[Serializable]
public class UpgradeSpecData
{
    public EStatType StatType;
    public EModifierType ModifierType;
    public int MaxLevel;
    public int BaseCost;
    public int CostMultiplier;
    public int BaseStat;
    public double Value;
    public string Name;
    public string Description;
}
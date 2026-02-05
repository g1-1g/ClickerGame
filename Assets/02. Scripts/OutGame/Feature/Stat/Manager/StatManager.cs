using System;
using UnityEngine;
using static UnityEditor.Progress;

public class StatManager : MonoBehaviour
{
    private static StatManager _instance;
    public static StatManager Instance { get { return _instance; } }

    public double[] _stats = new double[(int)EItemType.HeartPerClick];

    public static event Action OnStatChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }


    // 업그레이드 적용
    public void ApplyLevel(Item item)
    {
        

        OnStatChanged?.Invoke();
    }

    // 최종 스탯 값 계산
    public double GetStat(EItemType statType)
    {
        double additiveBonus = 0;
        double multiplicativeBonus = 1f;
        
        var data =  ItemManager.Instance.Get(statType).SpecData;
        var level = ItemManager.Instance.Get(statType).Level;

        if (data.ModifierType == EModifierType.Multiplicative)
        {
            multiplicativeBonus *= level == 1 ? 1 : (data.Value * (double)(level));
        }
        else if (data.ModifierType == EModifierType.Additive)
        {
            additiveBonus += (data.Value * (double)(level - 1));
        }

        _stats[(int)data.StatType] = (data.BaseStat + additiveBonus) * multiplicativeBonus;

        return _stats[(int)statType];
    }
}

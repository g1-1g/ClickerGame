using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance;
    public static UpgradeManager Instance { get { return _instance; } }

    public static event Action<EStatType> OnDataChanged;

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EStatType, Upgrade> _upgrades = new();

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        // 스펙 데이터에 따라 도메인 생성
        foreach (var specData in _specTable.Datas)
        {
            if (_upgrades.ContainsKey(specData.StatType))
            {
                throw new Exception($"There is already an upgrade with type {specData.StatType}");
            }

            _upgrades.Add(specData.StatType, new Upgrade(specData));

            OnDataChanged?.Invoke(specData.StatType);
        }
    }

    public Upgrade Get(EStatType type) => _upgrades[type] ?? null;
    public List<Upgrade> GetAll() => _upgrades.Values.ToList();

    public bool CanLevelUp(EStatType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        if (!upgrade.CanLevelUp())
        {
            return false;
        }

        return CurrencyManager.Instance.CanAfford(ECurrencyType.Heart, upgrade.Cost);
    }

    public bool TryLevelUp(EStatType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }

        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Heart, upgrade.Cost))
        {
            return false;
        }

        if (!upgrade.TryLevelUp())
        {
            return false;
        }

        StatManager.Instance.ApplyUpgrade(upgrade.SpecData);
        OnDataChanged?.Invoke(type);

        return true;
    }
}
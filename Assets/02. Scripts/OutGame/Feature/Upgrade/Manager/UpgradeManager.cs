using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance;
    public static UpgradeManager Instance { get { return _instance; } }

    public static event Action<EUpgradeType> OnDataChanged;

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EUpgradeType, Upgrade> _upgrades = new();

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
            if (_upgrades.ContainsKey(specData.Type))
            {
                throw new Exception($"There is already an upgrade with type {specData.Type}");
            }

            _upgrades.Add(specData.Type, new Upgrade(specData));

            OnDataChanged?.Invoke(specData.Type);
        }
    }

    public Upgrade Get(EUpgradeType type) => _upgrades[type] ?? null;
    public List<Upgrade> GetAll() => _upgrades.Values.ToList();

    public bool CanLevelUp(EUpgradeType type)
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

    public bool TryLevelUp(EUpgradeType type)
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

        OnDataChanged?.Invoke(type);

        return true;
    }
}
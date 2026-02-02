using System;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    private static StatManager _instance;
    public static StatManager Instance { get { return _instance; } }

    [SerializeField]
    private double[] _baseStats = new double[(int)EStatType.Count];

    private double[] _stats = new double[(int)EStatType.Count];

    public double HeartPerClick => _stats[(int)EStatType.HeartPerClick];


    public static event Action OnStatChanged;

    private IStatRepository _repository;

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        _repository = new StatRepository("AccountManager.Instance.Email");
    }

    private void Start()
    {
        LoadData();
    }


    // 업그레이드 적용
    public void ApplyUpgrade(UpgradeSpecData upgradeData)
    {
        double additiveBonus = 0;
        double multiplicativeBonus = 1f;

        if (upgradeData.ModifierType == EModifierType.Multiplicative)
        {
            multiplicativeBonus *= upgradeData.Value;
        }
        else if (upgradeData.ModifierType == EModifierType.Additive){
            additiveBonus += upgradeData.Value;
        }

        var baseValue = _baseStats[(int)upgradeData.StatType];

        _stats[(int)upgradeData.StatType] = (_stats[(int)upgradeData.StatType] + additiveBonus) * multiplicativeBonus;

        SaveData();
        OnStatChanged?.Invoke();
    }

    // 최종 스탯 값 계산
    public double GetStat(EStatType statType)
    {
        return _stats[(int)statType];
    }


    private void LoadData()
    {
        var loadedData = _repository.Load();

        if (loadedData == null)
        {
            // 데이터 없으면 기본값으로 초기화
            Debug.Log("저장된 데이터 없음 - 기본값 생성");
            _stats = CreateBaseStatData().Stats;

            // 즉시 저장 (다음부터는 로드됨)
            SaveData();
        }
        else
        {
            _stats = loadedData.Stats;
            Debug.Log("데이터 로드 완료");
        }
    }

    private StatSaveData CreateBaseStatData()
    {
        StatSaveData data = new StatSaveData();
        data.Stats = _baseStats;

        return data;
    }

    private void SaveData()
    {
        StatSaveData data = new StatSaveData();
        data.Stats = _stats;

        _repository.Save(data);
    }
}

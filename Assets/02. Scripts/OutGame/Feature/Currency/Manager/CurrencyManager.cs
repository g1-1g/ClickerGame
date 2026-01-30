using System;
using UnityEngine;


public class CurrencyManager : MonoBehaviour
{
    //CRUD
    //재화에 대한 생성 / 조회 / 사용 / 소모 / 이벤트

    private static CurrencyManager _instance;
    public static CurrencyManager Instance { get { return _instance; } }

    private double[] _currencies = new double[(int)ECurrencyType.Count];
    
    public double Heart => _currencies[(int)ECurrencyType.Heart];

    public static Action<EUpgradeType> OnDataChanged { get; internal set; }

    public static event Action OnCurrencyChanged;

    private CurrencyRepository _repository;

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        _repository = new CurrencyRepository();
    }

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        _currencies = _repository.Load().Currencies;
    }

    private void SaveData()
    {
        CurrencySaveData data = new CurrencySaveData();
        data.Currencies = _currencies;

        _repository.Save(data);
    }


    public void Add(ECurrencyType type, double amount)
    {
        _currencies[(int)type] += amount;

        CatManager.Instance.CurrentCat.AffectionUp(amount);
        OnCurrencyChanged?.Invoke();
        SaveData();
    }

    public bool TrySpend(ECurrencyType type, double amount)
    {
        if (amount > _currencies[(int)type])
        {
            return false;
        }

        _currencies[(int)type] -= amount;
        OnCurrencyChanged?.Invoke();
        SaveData();
        return true;
    }

    public bool CanAfford(ECurrencyType type, double amount)
    {
        if (amount > _currencies[(int)type])
        {
            return false;
        }

        return true;
    }
}

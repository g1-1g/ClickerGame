using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;
    public static CurrencyManager Instance { get { return _instance; } }

    private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
    private ICurrencyRepository _repository;
    private bool _isReady = false;

    public double Heart => _currencies[(int)ECurrencyType.Heart].Value;

    public static event Action OnDataChanged;
    public static event Action<double> OnCurrencyAdded;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public async UniTask Initialize(ICurrencyRepository repository)
    {
        if (_isReady)
        {
            return;
        }

        _repository = repository;
        await LoadData();

        _isReady = true;
    }

    private async UniTask LoadData()
    {
        CurrencySaveData data = await _repository.Load();
        double[] savedCurrencies = data?.Currencies ?? CurrencySaveData.Default.Currencies;

        for (int i = 0; i < _currencies.Length; i++)
        {
            double value = i < savedCurrencies.Length ? savedCurrencies[i] : 0;
            _currencies[i] = new Currency(value);
        }

        OnDataChanged?.Invoke();
    }

    private void SaveData()
    {
        if (!_isReady) return;

        CurrencySaveData data = new CurrencySaveData();
        data.Currencies = new double[_currencies.Length];

        for (int i = 0; i < _currencies.Length; i++)
        {
            data.Currencies[i] = _currencies[i].Value;
        }

        _repository.Save(data);
    }

    public void Add(ECurrencyType type, double amount)
    {
        Currency amountCurrency = new Currency(amount);
        _currencies[(int)type] = _currencies[(int)type] + amountCurrency;

        OnDataChanged?.Invoke();
        OnCurrencyAdded?.Invoke(amount);
        SaveData();
    }

    public bool TrySpend(ECurrencyType type, Currency amount)
    {
        if (amount > _currencies[(int)type])
        {
            return false;
        }

        _currencies[(int)type] = _currencies[(int)type] - amount;
        OnDataChanged?.Invoke();
        SaveData();
        return true;
    }

    public bool CanAfford(ECurrencyType type, Currency amount)
    {
        if (amount > _currencies[(int)type])
        {
            return false;
        }

        return true;
    }
}

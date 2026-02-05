using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class CurrencyManager : MonoBehaviour
{
    //CRUD
    //재화에 대한 생성 / 조회 / 사용 / 소모 / 이벤트

    private static CurrencyManager _instance;
    public static CurrencyManager Instance { get { return _instance; } }

    private double[] _currencies = new double[(int)ECurrencyType.Count];
    
    public double Heart => _currencies[(int)ECurrencyType.Heart];

    public static event Action OnDataChanged;
    public static event Action<double> OnCurrencyAdded;

    private ICurrencyRepository _repository;

    private bool _isReady = false;
    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        FirebaseInitializer.OnFirebaseInitialized += Init;
    }

    private async void Init()
    {
         // Repository 생성
        _repository = new FirebaseCurrencyRepository(AccountManager.Instance.Email);

        // 데이터 로드
        await LoadData();

        _isReady = true;
    }

    private async UniTask LoadData()
    {
        CurrencySaveData data = await _repository.Load();
        _currencies = data.Currencies;
        OnDataChanged?.Invoke();
    }

    private void SaveData()
    {
        if (!_isReady) return;
        CurrencySaveData data = new CurrencySaveData();
        data.Currencies = _currencies;

        _repository.Save(data);
    }


    public void Add(ECurrencyType type, double amount)
    {
        _currencies[(int)type] += amount;

        CatManager.Instance.AffectionUp(amount);
        OnDataChanged?.Invoke();
        OnCurrencyAdded?.Invoke(amount);
        SaveData();
    }

    public bool TrySpend(ECurrencyType type, double amount)
    {
        if (amount > _currencies[(int)type])
        {
            return false;
        }

        _currencies[(int)type] -= amount;
        OnDataChanged?.Invoke();
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

    public void OnDestroy()
    {
        FirebaseInitializer.OnFirebaseInitialized -= Init;
    }
}

using System;
using UnityEngine;

public enum ECurrencyType
{
    Heart,
    Count,
}
public class CurrencyManager : MonoBehaviour
{
    //CRUD
    //재화에 대한 생성 / 조회 / 사용 / 소모 / 이벤트

    private static CurrencyManager _instance;
    public static CurrencyManager Instance { get { return _instance; } }

    private double[] _currencies = new double[(int)ECurrencyType.Count];
    
    public double Heart => _currencies[(int)ECurrencyType.Heart];

    public event Action<double> OnHeartChange;



    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void Add(ECurrencyType type, double amount)
    {
        _currencies[(int)type] += amount;

        GameManager.Instance.CurrentCat.AffectionUp(amount);
        OnHeartChange?.Invoke(Heart);
    }

    public bool TrySpendHeart(ECurrencyType type, double amount)
    {
        if (amount > _currencies[(int)type])
        {
            return false;
        }

        _currencies[(int)type] -= amount;
        OnHeartChange?.Invoke(Heart);
        return true;
    }
}

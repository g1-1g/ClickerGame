using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalCurrencyRepository : ICurrencyRepository
{
    string _userID;

    public LocalCurrencyRepository(string userID)
    {
        _userID = userID;
    }

    public async UniTask Save(CurrencySaveData data)
    {
        for (int i = 0; i < data.Currencies.Length; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString($"{_userID}_{type.ToString()}", data.Currencies[(int)type].ToString());
        }
    }

    public async UniTask<CurrencySaveData> Load()
    {
        CurrencySaveData data = CurrencySaveData.Default;
        for (int i = 0; i < data.Currencies.Length; i++)
        {
            var type = (ECurrencyType)i;
            if (double.TryParse(PlayerPrefs.GetString($"{_userID}_{type.ToString()}", "0"), out double value))
            {
                data.Currencies[(int)type] = value;
            }
        }

        return data;
    }
}



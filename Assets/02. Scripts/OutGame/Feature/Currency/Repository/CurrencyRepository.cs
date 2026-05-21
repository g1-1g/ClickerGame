using Cysharp.Threading.Tasks;
using UnityEngine;

public class CurrencyRepository : ICurrencyRepository
{
    public async UniTask Save(CurrencySaveData data)
    {
        for (int i = 0; i < data.Currencies.Length; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString(type.ToString(), data.Currencies[(int)type].ToString());
        }
    }

    public async UniTask<CurrencySaveData> Load()
    {
        CurrencySaveData data = CurrencySaveData.Default;
        for (int i = 0; i < data.Currencies.Length; i++)
        {
            var type = (ECurrencyType)i;
            if (double.TryParse(PlayerPrefs.GetString(type.ToString(), "0"), out double value))
            {
                data.Currencies[(int)type] = value;
            }
        }

        return data;
    }
}



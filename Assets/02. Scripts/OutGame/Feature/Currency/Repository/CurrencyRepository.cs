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
            data.Currencies[(int)type] = double.Parse(PlayerPrefs.GetString(type.ToString()));
        }

        return data;
    }
}



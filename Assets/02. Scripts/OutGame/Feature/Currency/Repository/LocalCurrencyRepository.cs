using UnityEngine;

public class LocalCurrencyRepository : ICurrencyRepository
{
    string _userID;

    public LocalCurrencyRepository(string userID)
    {
        _userID = userID;
    }

    public void Save(CurrencySaveData data)
    {
        for (int i = 0; i < data.Currencies.Length; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString($"{_userID}_{type.ToString()}", data.Currencies[(int)type].ToString());
        }
    }

    public CurrencySaveData Load()
    {
        CurrencySaveData data = CurrencySaveData.Default;
        for (int i = 0; i < data.Currencies.Length; i++)
        {
            var type = (ECurrencyType)i;
            data.Currencies[(int)type] = double.Parse(PlayerPrefs.GetString($"{_userID}_{type.ToString()}"));
        }

        return data;
    }
}



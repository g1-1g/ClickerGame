using UnityEngine;

public class StatRepository : IStatRepository
{
    private string _userId;
    public StatRepository(string userId)
    {
        _userId = userId;
    }

    public void Save(StatSaveData data)
    {
        for (int i = 0; i < data.Stats.Length; i++)
        {
            var type = (EStatType)i;
            PlayerPrefs.SetString($"{_userId}_{type.ToString()}", data.Stats[(int)type].ToString());
        }
    }

    public StatSaveData Load()
    {
        StatSaveData data = StatSaveData.Default;
        for (int i = 0; i < data.Stats.Length; i++)
        {
            var type = (EStatType)i;
            if (double.TryParse(PlayerPrefs.GetString($"{_userId}_{type.ToString()}"), out double value))
            {
                data.Stats[(int)type] = value;
            }
            else
            {
                return null;
            }
        }
        return data;
    }
}



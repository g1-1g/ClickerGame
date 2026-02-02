using UnityEngine;

public class ItemLevelRepository : IItemLevelRepository
{
    private string _userId;
    public ItemLevelRepository(string userId)
    {
        _userId = userId;
    }

    public void Save(ItemLevelSaveData data)
    {
        for (int i = 0; i < data.Levels.Length; i++)
        {
            var type = (EItemType)i;
            PlayerPrefs.SetString($"{_userId}_{type.ToString()}", data.Levels[(int)type].ToString());
        }
    }

    public ItemLevelSaveData Load()
    {
        ItemLevelSaveData data = ItemLevelSaveData.Default;
        for (int i = 0; i < data.Levels.Length; i++)
        {
            var type = (EItemType)i;
            if (int.TryParse(PlayerPrefs.GetString($"{_userId}_{type.ToString()}"), out int value))
            {
                data.Levels[(int)type] = value;
            }
            else
            {
                return null;
            }
        }
        return data;
    }
}



using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemLevelRepository : IItemLevelRepository
{
    private string _userId;
    public ItemLevelRepository(string userId)
    {
        _userId = userId;
    }

    public UniTask Save(ItemLevelSaveData data)
    {
        for (int i = 0; i < data.Levels.Length; i++)
        {
            var type = (EItemType)i;
            PlayerPrefs.SetString($"{_userId}_{type.ToString()}", data.Levels[(int)type].ToString());
        }
        return UniTask.CompletedTask;
    }

    public UniTask<ItemLevelSaveData> Load()
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
                data = null;
                UniTask.FromResult(data);
            }
        }
        return UniTask.FromResult(data);
    }
}



using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IItemLevelRepository
{
    public UniTask Save(ItemLevelSaveData data);

    public UniTask<ItemLevelSaveData> Load();
    
}

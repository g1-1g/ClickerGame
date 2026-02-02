using UnityEngine;

public interface IItemLevelRepository
{
    public void Save(ItemLevelSaveData data);

    public ItemLevelSaveData Load();
    
}

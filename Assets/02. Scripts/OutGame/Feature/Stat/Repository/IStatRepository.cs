using UnityEngine;

public interface IStatRepository
{
    public void Save(StatSaveData data);

    public StatSaveData Load();
    
}

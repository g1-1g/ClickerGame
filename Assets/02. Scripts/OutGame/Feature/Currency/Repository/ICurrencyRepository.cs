using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ICurrencyRepository
{
    public UniTask Save(CurrencySaveData saveData);

    public UniTask<CurrencySaveData> Load();
    
}

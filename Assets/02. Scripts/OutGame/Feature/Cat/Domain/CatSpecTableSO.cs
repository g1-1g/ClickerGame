using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;


[CreateAssetMenu(fileName = "CatSpecTableSO", menuName = "ScriptableObjects/CatSpecTableSO", order = 0)]
public class CatSpecTableSO : ScriptableObject
{
    [Header("고양이 정의 목록")]
    [SerializeField]
    private CatSpecDataSO[] _cats;

    public CatSpecDataSO GetCatData(CatSaveData data)
    {
        return _cats[(int)data.CatType];
    }

    public CatLevelSpecData GetCatLevelData(CatSaveData data)
    {
        _cats[(int)data.CatType].TryGetLevelData(data.Level, out var spec);
        return spec;
    }

    public int GetMaxLevel(CatSaveData data)
    {
        return _cats[(int)data.CatType].GetMaxLevel();
    }
}

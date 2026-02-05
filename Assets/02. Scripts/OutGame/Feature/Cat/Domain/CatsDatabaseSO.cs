using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;


[CreateAssetMenu(fileName = "CatsDatabaseSO", menuName = "ScriptableObjects/CatsDatabaseSO", order = 0)]
public class CatsDatabaseSO : ScriptableObject
{
    [Header("고양이 데이터 목록")]
    [SerializeField]
    private CatDatabaseSO[] _cats;

    public CatDatabaseSO GetCatData(CatData data)
    {
        return _cats[(int)data.CatType];
    }

    public CatLevelDataSO GetCatLevelData(CatData data)
    {
        return _cats[(int)data.CatType].GetLevelData(data.Level);
    }

    public int GetMaxLevel(CatData data)
    {
        return _cats[(int)data.CatType].GetMaxLevel();
    }
}

using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;


[CreateAssetMenu(fileName = "CatSpecTableSO", menuName = "ScriptableObjects/CatSpecTableSO", order = 0)]
public class CatSpecTableSO : ScriptableObject
{
    [Header("고양이 정의 목록")]
    [SerializeField]
    public CatSpecDataSO[] Cats;

    public CatSpecDataSO GetCatData(ECatType catType)
    {
        foreach (var cat in Cats)
        {
            if (cat.CatType == catType)
            {
                return cat;
            }
        }
        return null;
    }

    public CatLevelSpecData GetCatLevelData(ECatType catType, int level)
    {
        foreach (var cat in Cats)
        {
            if (cat.CatType == catType)
            {
                cat.TryGetLevelData(level, out var spec);
                return spec;
            }
        }
        Debug.LogWarning($"{catType}에 대한 Data가 없습니다.");
        return default;
    }

    public int GetMaxLevel(ECatType catType)
    {
        foreach (var cat in Cats)
        {
            if (cat.CatType == catType)
            {
                return cat.GetMaxLevel();
            }
        }
        Debug.LogWarning($"{catType}에 대한 Data가 없습니다.");
        return 0;
    }
}

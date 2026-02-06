using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatSpecTableSO", menuName = "ScriptableObjects/CatSpecTableSO", order = 0)]
public class CatSpecTableSO : ScriptableObject
{
    [Header("고양이 정의 목록")]
    [SerializeField]
    public CatSpecDataSO[] Cats;

    private Dictionary<ECatType, CatSpecDataSO> _cats =  new Dictionary<ECatType, CatSpecDataSO>();

    private void OnEnable()
    {
        _cats.Clear();
        foreach (var cat in Cats)
        {
            _cats[cat.CatType] = cat;
        }
    }

    public CatSpecDataSO GetCatData(ECatType catType)
    {
        _cats.TryGetValue(catType, out var cat);
        return cat;
    }

    public CatLevelSpecData GetCatLevelData(ECatType catType, int level)
    {
        if (_cats.TryGetValue(catType, out var cat)){
            cat.TryGetLevelData(level, out var spec);
            return spec;
        }

            Debug.LogWarning($"{catType}에 대한 Data가 없습니다.");
            return default;
    }

    public int GetMaxLevel(ECatType catType)
    {
        if (_cats.TryGetValue(catType, out var cat))
        {
            return cat.GetMaxLevel();
        }

        Debug.LogWarning($"{catType}에 대한 Data가 없습니다.");
        return 0;
    }
}

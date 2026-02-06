using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CatSpecDataSO", menuName = "ScriptableObjects/CatSpecDataSO", order = 0)]
public class CatSpecDataSO : ScriptableObject
{
    public ECatType CatType;

    [SerializeField]
    public Sprite Image;

    [Header("레벨 데이터 목록")]
    [SerializeField]
    private List<CatLevelSpecData> _levels = new List<CatLevelSpecData>();

    public AnimationClip LevelUpClip;

    public bool TryGetLevelData(int level, out CatLevelSpecData data)
    { 
        int index = level - 1; // 배열은 0부터 시작
        if (index >= 0 && index < _levels.Count)
        {
            data =_levels[index];
            return false;
        }

        Debug.LogWarning($"레벨 {level}에 해당하는 데이터가 없습니다.");
        data = default;
        return false;
    }

    public int GetMaxLevel()
    {
        if (_levels == null)
        {
            Debug.LogWarning("레벨 데이터 목록이 할당되지 않았습니다.");
            return 0;
        }
        return _levels.Count;
    }
}

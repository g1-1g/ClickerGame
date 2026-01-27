using UnityEngine;


[CreateAssetMenu(fileName = "CatLevelDatabaseSO", menuName = "ScriptableObjects/CatLevelDatabaseSO", order = 0)]
public class CatLevelDatabaseSO : ScriptableObject
{
    [Header("레벨 데이터 목록")]
    [SerializeField]
    private CatLevelDataSO[] _levels;

    public AnimationClip LevelUpClip;

    public CatLevelDataSO GetLevelData(int level)
    {
        int index = level - 1; // 배열은 0부터 시작
        if (index >= 0 && index < _levels.Length)
        {
            return _levels[index];
        }

        Debug.LogWarning($"레벨 {level}에 해당하는 데이터가 없습니다.");
        return null;
    }

    public int GetMaxLevel()
    {
        return _levels.Length;
    }
}

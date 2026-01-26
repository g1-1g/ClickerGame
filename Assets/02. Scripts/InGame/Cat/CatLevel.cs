using System;
using UnityEngine;

public class CatLevel : MonoBehaviour
{
    private int _level;
    public int Level {  get { return _level; } }

    [SerializeField]
    private CatLevelDatabaseSO _catLevelsDatabase;

    private CatLevelDataSO _currentLevelData;

    public event Action<CatLevelDataSO> OnLevelChanged;
    public void LevelUp()
    {
        _level++;
        _currentLevelData = _catLevelsDatabase.GetLevelData(_level);
        OnLevelChanged?.Invoke(_currentLevelData);
    }

    private void Awake()
    {
        _level = 0;
    }

    private void Start()
    {
        LevelUp();
    }
}
